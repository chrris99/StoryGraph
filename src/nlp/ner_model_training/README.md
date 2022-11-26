# ner_model_training

This submodule is used to train a BiLSTM+CRF-based NER tagger using the NerKor open-source dataset.


## Exploring the data & creating train/dev splits

The dataset can be found at: https://github.com/nytud/NYTK-NerKor

The notebook `notebooks/prepare_ner_kor_data.ipynb` contains all the necessary code required to load the data from the above Github repository and create the train/dev/test splits.

The notebook `notebooks/visualization.ipynb` contains some simple n-gram visualizations of the NerKor dataset.

## Model training

You are expected to have created a `train.tsv` and `dev.tsv` files using the notebook in Phase 1, which have the following schema:
```
sentence	token	tag
1	Bolondnak	O
1	tart	O
1	?	O
2	Ha	O
2	a	O
```

These files should be uploaded under the following folder structure: `data/ner/hu/1.0`. 
You also need to download an embedding file from the internet, 
we selected one from here: https://github.com/oroszgy/awesome-hungarian-nlp
(HuSpaCy 100d 100d Floret embeddings trained on the Hungarian Webcorpus 2.0)

The training can be run in two consecutive steps, which are detailed below.

## Configuring the training script
Both steps of the training use a shared `yaml` config specifying all the required parameters. 
An example can be found at `data/ner/hu/1.0/train_ner.yml`.

## Data preparation
An initial script creates word-to-index, char-to-index and label-to-index mappings and saves them to tsv files, 
which are loaded in the second step of training. This script also saves the vectorizer to a pkl file 
and shrinks the embeddings to only contain tokens that are present in the train and dev sets.

You can run this script with the following command:
```bash
PYTHONPATH=. python src/entrypoints/prepare_data.py <path_to_your_config_file>
```

## Model training
The repository implements a biLSTM+CRF-based sequence tagger that can be used to train an NER model.
To launch model training, you need to run:
```bash
 PYTHONPATH=.  python src/entrypoints/train.py <path_to-your_config_file>
```

The training script will print the validation accuracy and the classification report at the end of every epoch.

### Best model scores
Scores are computed with the seqeval library.
For the Software Architectures homework we will only be relying on `PER` labels.
```
val_fscore: 67.67
              precision    recall  f1-score   support

         LOC       0.76      0.78      0.77      3153
        MISC       0.55      0.30      0.39      1458
         ORG       0.70      0.81      0.75      3125
         PER       0.72      0.88      0.80      3694

   micro avg       0.72      0.76      0.74     11430
   macro avg       0.68      0.69      0.68     11430
weighted avg       0.71      0.76      0.72     11430
```




