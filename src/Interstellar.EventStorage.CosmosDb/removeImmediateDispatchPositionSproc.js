/*
 * A Cosmos DB stored procedure that removes the immediate dispatch position by setting it to the last index.<br/>
 *
 * It may may if the last index passed in does not match the last index on the metadata, if that is the case its because 
 * another write to the stream has occured in between writing events and this call, so do nothing.
 * @function
 * @param {string} streamId - id of the stream
 * @param {int} lastEventIndex - index of the last event in the stream
 */

function removeImmediateDispatchPositionSproc(streamId, lastEventIndex) {
    const ERROR_CODES = {
        BAD_REQUEST: 400,
        CONFLICT: 409,
        NOT_ACCEPTED: 499
    };

    var collection = getContext().getCollection();
    var collectionLink = collection.getSelfLink();

    if (streamId === undefined) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "The streamId is undefined or null.");
    }

    if (lastEventIndex === undefined) {
        throw new Error(ERROR_CODES.BAD_REQUEST, "The lastEventIndex is undefined or null.");
    }

    var lastEventInStreamQuery = {
        query: 'SELECT * FROM c WHERE c.StreamId = @streamId ORDER BY c.EventIndex DESC',
        parameters: [{
            name: '@streamId',
            value: streamId
        }]
    };

    collection.queryDocuments(collectionLink, lastEventInStreamQuery, function (err, items) {
        if (err) {
            throw err;
        }

        if (items[0] === lastEventIndex) {
            updateMetadata(items[0].EventIndex);
        } else {
            throw new Error(ERROR_CODES.CONFLICT, "lastEventIndex: " + lastEventIndex + " differs from stream LastIndex: " + items[0].EventIndex + ".");
        }
    });

    function updateMetadata() {
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

            if (items.length > 0) {
                updateMetadata(items[0]);
            } else {
                throw new Error(ERROR_CODES.NOT_ACCEPTED, "Could not find stream metadata.");
            }
        });
    }

    function updateMetadata(metadata) {
        var newMetadata = {
            id: metadata.id,
            StreamId: streamId,
            ImmediateUpdateEventIndex: lastEventIndex
        };

        if (!collection.replaceDocument(metadata._self, newMetadata, updateMetadataCallback)) {
            throw new Error(ERROR_CODES.NOT_ACCEPTED, "Could not write stream metadata.");
        }
    }

    function updateMetadataCallback(err) {
        if (err) {
            throw err;
        }
        getContext().getResponse().setBody(0);
    } 

    function getStreamMetadataId() {
        return 'stream-metadata-' + streamId;
    }
}