## Sentence splitter microservice


### Building the Docker image
```bash
docker build . -t sw-arch-nlp-sentence-splitter:latest
```


### Running the containerized web service
```bash
docker docker run -p 5000:5000 sw-arch-nlp-sentence-splitter:latest
```


### Expected input payload schema
```bash
{
  "document": "Ez egy magyar mondat. Ez még egy. Ez egy harmadik, amiben Ady Endre is benne van :D"
}
```

### Output payload schema
```bash
["Ez egy magyar mondat.",
 "Ez még egy.",
 "Ez megy egy harmadik, amiben Ady Endre is benne van :D"]
```