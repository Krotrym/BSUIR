FROM python:3.11-slim

WORKDIR /app

COPY app/requirements.txt /app/requirements.txt

# установить только git (минимально) и pip зависимости
RUN apt-get update \
    && apt-get install -y --no-install-recommends git \
    && rm -rf /var/lib/apt/lists/* \
    && pip install --no-cache-dir --upgrade pip \
    && pip install --no-cache-dir -r /app/requirements.txt

COPY app /app

VOLUME ["/data"]
ENV CONFIG_PATH=/data/input.json

CMD ["python", "main.py"]
