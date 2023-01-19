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

    checkSliceCanBeWritten();

    function checkSliceCanBeWritten() {
        var lastEventInStreamQuery = {
            query: 'SELECT * FROM c WHERE c.StreamId = @streamId ORDER BY c.EventIndex DESC',
            parameters: [{
                name: '@streamId',
                value: eventSlice.StreamId
            }]
        };

        collection.queryDocuments(collectionLink, lastEventInStreamQuery, function (err, items) {
            if (err) {
                throw err;
            }
            if (items.length === 0) {
                createMetadata();
            } else if (items[0].EventIndex === eventSlice.StartIndex) {
                writeEvents();
            } else {
                throw new Error(ERROR_CODES.CONFLICT, "Slice StartIndex: " + eventSlice.StartIndex + " differs from stream LastIndex: " + items[0].EventIndex + ".");
            }
        });
    }
       
    function createMetadata() {
        var streamMetadata = {
            id: getStreamMetadataId(),
            StreamId: eventSlice.StreamId,
            ImmediateUpdateEventIndex: -1
        };
        if (!collection.createDocument(collectionLink, streamMetadata, options, createMetadataCallback)) {
            throw new Error(ERROR_CODES.NOT_ACCEPTED, "Could not write stream metadata.");
        }
    }

    function createMetadataCallback(err) {
        if (err) {
            throw err;
        }
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