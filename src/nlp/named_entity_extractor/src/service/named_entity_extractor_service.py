import traceback

import falcon

from backend.named_entity_extractor_pipeline import NamedEntityExtractorPipeline
import json
from logging import getLogger

logger = getLogger(__name__)


class SentenceSplitting:
    def __init__(self):
        self.config_path = 'ner_model_training/data/ner/hu/1.0/train_ner.yml'
        self.named_entity_extractor = NamedEntityExtractorPipeline(self.config_path)

    def extract_person_entities(self, payload):
        return self.named_entity_extractor.extract_person_entities_from_payload(payload)

    def on_post(self, request, response):
        try:
            payload = request.media
            result = self.extract_person_entities(payload)
            response.status = falcon.HTTP_200
            response.body = json.dumps({
               'statusCode': falcon.HTTP_200, 'body': result
            })
        except Exception as e:
            response.status = falcon.HTTP_400
            logger.error(f'Failed to process request: {request} with exception: {e}. Traceback: {traceback.format_exc()}')


def create_app():
    app = falcon.API()
    app.add_route('/extract_person_entities', SentenceSplitting())
    return app


application = create_app()
