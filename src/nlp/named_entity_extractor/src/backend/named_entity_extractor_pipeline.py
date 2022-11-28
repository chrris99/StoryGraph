from backend.inference import Inference


class NamedEntityExtractorPipeline:
    def __init__(self, config_path):
        self.predictor = Inference(config_path)

    def extract_person_entities_from_payload(self, payload):
        """Receives a JSON payload (output of token-enricher service)
        and returns the list of named entities belonging to the PER class"""
        token_list = [token['text'] for token in payload['tokens']]
        lemma_list = [token['lemma'] for token in payload['tokens']]
        output_labels = self.predictor.run([token_list])
        entities = []
        current_entity = None
        for idx, lemmatized_token in enumerate(lemma_list):
            if output_labels[0][idx] == "B-PER":
                current_entity = lemmatized_token
            elif output_labels[0][idx] == "I-PER" and current_entity is not None:
                current_entity += f' {lemmatized_token}'  # append part of entity with a space
            elif output_labels[0][idx] == "I-PER" and current_entity is None:
                # In case the model would predict "I-PER" as the first part of an entity
                current_entity = lemmatized_token
            if idx + 1 == len(output_labels[0]):
                # Reached end of token list
                if current_entity is not None:
                    entities.append(current_entity)
                continue
            elif output_labels[0][idx + 1] != "I-PER" and current_entity is not None:
                # Check if the current output label is not part a continuation of this entity
                entities.append(current_entity)
                current_entity = None

        return {
            'person_entities': entities
        }


if __name__ == '__main__':
    example_token_enricher_output_payload = {
        "body": {
            "tokens": [
                {
                    "text": "Az",
                    "universalPos": "DET",
                    "lemma": "az"
                },
                {
                    "text": "egyik",
                    "universalPos": "PRON",
                    "lemma": "egyik"
                },
                {
                    "text": "legnagyobb",
                    "universalPos": "ADJ",
                    "lemma": "nagy"
                },
                {
                    "text": "költőnk",
                    "universalPos": "NOUN",
                    "lemma": "költő"
                },
                {
                    "text": "Ady",
                    "universalPos": "PROPN",
                    "lemma": "Ady"
                },
                {
                    "text": "Endre",
                    "universalPos": "PROPN",
                    "lemma": "Endre"
                },
                {
                    "text": "volt",
                    "universalPos": "AUX",
                    "lemma": "van"
                },
                {
                    "text": "az",
                    "universalPos": "PRON",
                    "lemma": "az"
                },
                {
                    "text": ",",
                    "universalPos": "PUNCT",
                    "lemma": ","
                },
                {
                    "text": "aki",
                    "universalPos": "PRON",
                    "lemma": "aki"
                },
                {
                    "text": "személyesen",
                    "universalPos": "ADJ",
                    "lemma": "személyes"
                },
                {
                    "text": "írt",
                    "universalPos": "VERB",
                    "lemma": "ír"
                },
                {
                    "text": "levelet",
                    "universalPos": "NOUN",
                    "lemma": "levél"
                },
                {
                    "text": "Babits",
                    "universalPos": "PROPN",
                    "lemma": "Babits"
                },
                {
                    "text": "Mihálynak",
                    "universalPos": "PROPN",
                    "lemma": "Mihály"
                },
                {
                    "text": ".",
                    "universalPos": "PUNCT",
                    "lemma": "."
                }
            ]
        }
    }
    config_path = '/Users/attilanagy/Personal/StoryGraph/src/nlp/named_entity_extractor/src/ner_model_training/data/ner/hu/1.0/train_ner.yml'
    named_entity_extractor = NamedEntityExtractorPipeline(config_path)

    extracted_person_entities = named_entity_extractor.extract_person_entities_from_payload(
        example_token_enricher_output_payload)
    print(extracted_person_entities)
