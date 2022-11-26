import traceback

import falcon

from backend.token_enricher_pipeline import TokenEnricherPipeline
import json
from logging import getLogger

logger = getLogger(__name__)


class TokenEnrichment:
    def __init__(self):
        self.token_enricher = TokenEnricherPipeline()

    def enrich_tokens(self, payload):
        return self.token_enricher.enrich_sentence_payload(payload)

    def on_post(self, request, response):
        try:
            payload = request.media
            result = self.enrich_tokens(payload)
            response.status = falcon.HTTP_200
            response.body = json.dumps({
               'statusCode': falcon.HTTP_200, 'body': result
            })
        except Exception as e:
            response.status = falcon.HTTP_400
            logger.error(f'Failed to process request: {request} with exception: {e}. Traceback: {traceback.format_exc()}')


def create_app():
    app = falcon.API()
    app.add_route('/enrich_tokens', TokenEnrichment())
    return app


application = create_app()
