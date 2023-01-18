/*
 * A Cosmos DB stored procedure that stores an event slice.<br/>
 *
 * @function
 * @param {object} eventSlice - Slice containing events to write, StreamId, and its strat position to wrie in the stream (StartIndex).
 */

function storeEventsSproc(eventSlice) {
    const ERROR_CODES = {
        BAD_REQUEST: 400,
        CONFLICT: 409,
        NOT_ACCEPTED: 499
    };

    var collection = getContext().getCollection();
    var collectionLink = collection.getSelfLink();

    if (eventSlice === undefined) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "The eventSlice is undefined or null.");
    }

    if (eventSlice.Events === undefined) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "The eventSlice Events are undefined or null.");
    }

    if (eventSlice.StreamId === undefined) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "The eventSlice StreamId is undefined or null.");
    }

    if (eventSlice.StartIndex === undefined) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "The eventSlice StartIndex is undefined or null.");
    }

    if (eventSlice.Events.length == 0) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "There are no events in the slice to write.");
    }

    var options = { disableAutomaticIdGeneration: false };
    var eventWriteCount = 0;
    var metadata;

    checkSliceCanBeWritten();

    function checkSliceCanBeWritten() {
        var streamMetadataQuery = {
            query: 'SELECT * FROM c WHERE c.id = @metadataId',
            parameters: [{
                name: '@metadataId',
                value: getStreamMetadataId()
            }]
        };

        collection.queryDocuments(collectionLink, streamMetadataQuery, function (err, items) {
            if (err) {
                throw err;
            }
            if (items.length === 0) {
                createMetadata();
            } else if (items[0].LastEventIndex === eventSlice.StartIndex) {
                metadata = items[0];
                writeEvents();
            } else {
                throw new Error(ERROR_CODES.CONFLICT, "Slice StartIndex differs from stream LastIndex.");
            }
        });
    }

    function createMetadata() {
        var streamMetadata = {
            id: getStreamMetadataId(),
            StreamId: eventSlice.StreamId,
            LastEventIndex: -1,
            ImmediateUpdateEventIndex: -1
        };
        if (!collection.createDocument(collectionLink, streamMetadata, options, createMetadataCallback)) {
            throw new Error(ERROR_CODES.NOT_ACCEPTED, "Could not write stream metadata.");
        }
    }

    function createMetadataCallback(err, item) {
        if (err) {
            throw err;
        }
        metadata = item;
        writeEvents();
    }

    function writeEvents() {
        writeEvent(eventSlice.Events[0]);
    }

    function writeEvent(eventPayload) {
        if (!collection.createDocument(collectionLink, eventPayload, options, eventWriteCallback)) {
            getContext().getResponse().setBody(eventWriteCount);
        }
    }

    function eventWriteCallback(err) {
        if (err) {
            throw err;
        }

        updateMetadata(eventSlice.Events[eventWriteCount]);
    }

    function updateMetadata(eventPayload) {
        var newMetadata = {
            id: metadata.id,
            StreamId: eventSlice.StreamId,
            LastEventIndex: eventPayload.EventIndex,
            ImmediateUpdateEventIndex: metadata.ImmediateUpdateEventIndex
        };

        if (!collection.replaceDocument(metadata._self, newMetadata, updateMetadataCallback)) {
            getContext().getResponse().setBody(eventWriteCount);
        }
    }

    function updateMetadataCallback(err) {
        if (err) {
            throw err;
        }

        eventWriteCount++;

        if (eventWriteCount === eventSlice.Events.length) {
            getContext().getResponse().setBody(eventWriteCount);
        } else {
            writeEvent(eventSlice.Events[eventWriteCount]);
        }
    }

    function getStreamMetadataId() {
        return 'stream-metadata-' + eventSlice.StreamId;
    }
}