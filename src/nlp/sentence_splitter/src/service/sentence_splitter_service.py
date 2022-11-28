import traceback

import falcon

from backend.sentence_splitter_pipeline import SentenceSplitterPipeline
import json
from logging import getLogger

logger = getLogger(__name__)


class SentenceSplitting:
    def __init__(self):
        self.sentence_splitter = SentenceSplitterPipeline()

    def split_sentence(self, payload):
        return self.sentence_splitter.split_doc_to_sentences_payload(payload)

    def on_post(self, request, response):
        try:
            payload = request.media
            result = self.split_sentence(payload)
            response.status = falcon.HTTP_200
            response.body = json.dumps(result)
        except Exception as e:
            response.status = falcon.HTTP_400
            logger.error(f'Failed to process request: {request} with exception: {e}. Traceback: {traceback.format_exc()}')


def create_app():
    app = falcon.API()
    app.add_route('/split_sentences', SentenceSplitting())
    return app


application = create_app()
