import spacy


class SentenceSplitterPipeline:
    def __init__(self):
        self.nlp_pipeline = spacy.load("hu_core_news_lg")

    def split_doc_to_sentences(self, document):
        """Receives a document as a string and returns the list of sentences."""
        doc = self.nlp_pipeline(document)
        splitted = [str(sentence) for sentence in doc.sents]
        return splitted

    def split_doc_to_sentences_payload(self, payload):
        """Receives a JSON payload
        and returns the list of sentences inside the response payload."""

        document = payload['document']
        splitted = self.split_doc_to_sentences(document)
        return splitted


if __name__ == '__main__':
    doc = "Ez egy magyar mondat. Ez még egy. Ez megy egy harmadik, amiben Ady Endre is benne van :D "
    payload_example = {'document': doc}
    sentence_splitter = SentenceSplitterPipeline()

    splitted_payload = sentence_splitter.split_doc_to_sentences_payload(payload_example)
    print(splitted_payload)
