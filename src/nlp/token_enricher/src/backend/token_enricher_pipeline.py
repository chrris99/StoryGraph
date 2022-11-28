import spacy

class TokenEnricherPipeline:
    def __init__(self):
        self.nlp_pipeline = spacy.load("hu_core_news_lg")

    def enrich_sentence(self, sentence):
        """Receives input sentence as str
        Returns """
        doc = self.nlp_pipeline(sentence)
        tokens = []

        # Input is guaranteed to be a single sentence
        sentence = [s for s in doc.sents][0]

        for token in sentence:
            token_info = {'text': str(token), 'universalPos': str(token.pos_), 'lemma': str(token.lemma_)}
            tokens.append(token_info)

        return tokens

    def enrich_sentence_payload(self, payload):
        """Receives a JSON payload
        and returns the list of token enrichments inside a response payload"""

        sentence = payload['sentence']
        enriched_tokens = self.enrich_sentence(sentence)
        return enriched_tokens


if __name__ == '__main__':
    sent = "Ez egy olyan mondat, amiben Ady Endrének a neve szerepel."
    token_enricher = TokenEnricherPipeline()
    enriched = token_enricher.enrich_sentence(sent)
