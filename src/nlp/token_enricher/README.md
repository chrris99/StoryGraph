## Token enricher microservice


### Building the Docker image
```bash
docker build . -t sw-arch-nlp-token-enricher:latest
```


### Running the containerized web service
```bash
docker run -p 5001:5001 sw-arch-nlp-token-enricher:latest
```


### Expected input payload schema
```bash
{
  "sentence": "Ez egy olyan mondat, amiben Ady Endrének a neve szerepel."
}
```

### Output payload schema
```bash
{
    "statusCode": "200 OK",
    "body": {
        "tokens": [
            {
                "text": "Ez",
                "universalPos": "PRON",
                "lemma": "ez"
            },
            {
                "text": "egy",
                "universalPos": "DET",
                "lemma": "egy"
            },
            {
                "text": "olyan",
                "universalPos": "ADJ",
                "lemma": "olyan"
            },
            {
                "text": "mondat",
                "universalPos": "NOUN",
                "lemma": "mondat"
            },
            {
                "text": ",",
                "universalPos": "PUNCT",
                "lemma": ","
            },
            {
                "text": "amiben",
                "universalPos": "PRON",
                "lemma": "ami"
            },
            {
                "text": "Ady",
                "universalPos": "PROPN",
                "lemma": "Ady"
            },
            {
                "text": "Endrének",
                "universalPos": "PROPN",
                "lemma": "Endre"
            },
            {
                "text": "a",
                "universalPos": "DET",
                "lemma": "a"
            },
            {
                "text": "neve",
                "universalPos": "NOUN",
                "lemma": "név"
            },
            {
                "text": "szerepel",
                "universalPos": "VERB",
                "lemma": "szerepel"
            },
            {
                "text": ".",
                "universalPos": "PUNCT",
                "lemma": "."
            }
        ]
    }
}
```