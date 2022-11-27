FROM python:3.6

#update
RUN apt-get update
RUN apt-get install -y

ENV POETRY_VERSION 1.1.13

# Install poetry
RUN curl -sSL https://raw.githubusercontent.com/sdispater/poetry/master/get-poetry.py > get-poetry.py && \
    python get-poetry.py --version $POETRY_VERSION
ENV PATH /root/.poetry/bin:$PATH

WORKDIR /tmp

# Configure & install dependencies with poetry
RUN poetry config virtualenvs.create false
COPY poetry.lock .
COPY pyproject.toml .
RUN poetry install --no-dev


#copy app
COPY src /src
WORKDIR /src


ENTRYPOINT ["gunicorn", "-w", "1", "-b", ":5002", "-t", "360", "--reload", "service.named_entity_extractor_service"]