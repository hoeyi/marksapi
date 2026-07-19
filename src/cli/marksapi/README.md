# marksapi
A unified command line interface for querying financial, economic, and relatedd data APIs.

**Current services**
- [Massive](https://www.massive.com/)

## Contents
- [Installation](#installation)
- [Configuration](#configuration)
- [Commands](#commands)
    - [General syntax](#general-syntax)
    - [MASSIVE](#massive)
    - [Output](#output)
- [Troubleshooting](#troubleshooting)
- [Attribution / Acknowledgements](#attribution--acknowledgements)

## Installation

### Building from source
Use the [Dockerfile](./Dockerfile) to build from source:

#### Bash
```bash
git clone https://github.com/hoeyi/markets-apiclient.git
cd markets-apiclient/src
docker build -t marksapi:latest -f cli/marksapi/build/Dockerfile .
```

You can now access the app by running the image interactively:
```bash
docker run -it marksapi:latest bash
```

and from within the container:

```bash
marksapi --version
```

The container will start as `root@<container_tag>` in the working directory `/app` by default. The working directory is added to `$PATH` during the Docker build.

### Using <em>docker compose</em>
You can also use `src/cli/marksapi//build/docker-compose.yml`. The volume mount provides an output location. Application logs are ephemeral unless copy to a mount location during the session.

#### docker-compose.yml
```yaml <>
services:
  marksapi:
    build:
      context: ../../../. # context: {project-root}/src/. 
      dockerfile: ./cli/marksapi/build/Dockerfile
    image: ghcr.io/hoeyi/marksapi:latest
    volumes:
    - ${HOME}/marksapi:/data
```

#### Bash
```bash
git clone https://github.com/hoeyi/markets-apiclient.git
cd markets-apiclient/src
# build the image then run
docker compose build marksapi
docker compose run marksapi
```

<sub>[Contents](#contents)</sub>

## Configuration

### Required
| Variable | Description |
|:--- |:--- |
| MASSIVE_API_KEY | API key for the **Massive** service. |

#### Use [docker secrets](https://docs.docker.com/engine/swarm/secrets/) to store sensitive data. 

**docker-compose.yml**
```yaml
services:
  marksapi:
  ...
  secrets:
    - MASSIVE_API_KEY

secrets:
  MASSIVE_API_KEY:
    file: ${HOME}/.docker-secrets/massive_api_key.txt 
```
This approach requires [Ichyd.Extensions.Configuration.Docker](https://github.com/ichyd/dotnet-Extensions.Configuration.Docker).

Alternatively, set your Massive API key as an environment variable (not recommended).
**Linux/macOS (bash/zsh)**
```bash
export MASSIVE_API_KEY="your_api_key_here"
```

**Windows PowerShell**
```powershell
$env:MASSIVE_API_KEY = "your_api_key_here"
```

**Windows CMD**
```bat
set MASSIVE_API_KEY=your_api_key_here
```

<sub>[Contents](#contents)</sub>

## Commands

### General Syntax

```bash
$ marksapi <service> <verb> [arguments] [options]
```

| Parameter | Description | Required | Default |
|---|---|---:|---|
| service | Service selector, e.g., massive | Yes | — |
| verb | Command verb, see below | Yes | — |
| arguments | Positional arguments depending on verb | Variable | — |
| options | Optional flags and parameters | No | — |

<sub>[Contents](#contents)</sub>

### massive 

#### Contents
- [aggregate-bar](#aggregate-bar)
- [short-volume](#short-volume)
- [ticker-info](#ticker-info)
- [tickers](#tickers)

#### aggregate-bar

Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for specified ticker(s).

**Usage:**
```bash
$ marksapi massive aggregate-bar <MARKET> <TICKER> --multiplier <INT> --timespan <ENUM> --from <DATE> --to <DATE> [--limit <INT>]
```

| Argument | Description | Required | Default |
|---|---|---:|---|
| MARKET | Market identifier (e.g., stocks, forex) | Yes | — |
| TICKER | Single case-sensitive ticker symbol | Yes | — |
| from | Start date of time window (ISO format: YYYY-MM-DD) | Yes | — |
| to | End date of time window (ISO format: YYYY-MM-DD) | Yes | — |
| multiplier | Timespan multiplier (e.g., 1 for 1 day, 5 for 5 days). Defaults to 1 if not provided. | No | — |
| timespan | Time window size (second, minute, hour, day, week, month, quarter, year). Defaults to 'day' if not provided. | No | — |
| limit | Maximum records to return | No | 100 |

| Option | Type | Range |
|---|---|---|
| --limit | Integer | Min: 1, Max: 1000 |

**Examples:**

**Daily candles for AAPL in 2024**
```bash
$ marksapi massive aggregate-bar stocks AAPL --multiplier 1 --timespan day --from 2024-01-01 --to 2024-12-31
```

**Weekly candles for multiple tickers**
```bash
$ marksapi massive aggregate-bar stocks --tickers AAPL,MSFT,GOOGL --multiplier 1 --timespan week --from 2024-01-01 --to 2024-12-31 --limit 500
```

**Hourly candles for Microsoft**
```bash
$ marksapi massive aggregate-bar stocks MSFT --multiplier 1 --timespan hour --from 2024-06-01 --to 2024-06-30 --limit 200
```

#### short-volume

Retrieve daily aggregated short sale volume data reported to FINRA from off-exchange trading venues and ATS.

**Usage:**
```bash
$ marksapi massive short-volume <TICKER> --from-date <DATE> --to-date <DATE> [--ratio-min <FLOAT>] [--ratio-max <FLOAT>] [--limit <INT>]
```

| Argument | Description | Required | Default |
|---|---|---:|---|
| TICKER | Primary ticker symbol | Yes | — |
| from-date | Start date of trade activity (YYYY-MM-DD) | Yes | — |
| to-date | End date of trade activity (YYYY-MM-DD) | Yes | — |
| ratio-min | Minimum short volume ratio filter | No | — |
| ratio-max | Maximum short volume ratio filter | No | — |
| limit | Maximum records to return | No | 10 |

| Option | Type | Range |
|---|---|---|
| --limit | Integer | Min: 1, Max: 50000 |

**Examples:**

**Get short volume for AAPL**
```bash
$ marksapi massive short-volume AAPL --from-date 2024-01-01 --to-date 2024-01-31
```

**Filter by ratio range for multiple tickers**
```bash
$ marksapi massive short-volume GME,AMC --from-date 2024-01-01 --to-date 2024-03-31 --ratio-min 0.3 --ratio-max 1.0 --limit 1000
```

**Bulk query with higher limit**
```bash
$ marksapi massive short-volume TSLA,NVDA,AMD --from-date 2024-06-01 --to-date 2024-06-30 --limit 5000
```

#### tickers

Query for tickers matching given conditions.

**Usage:**
```bash
$ marksapi massive tickers [--ticker <STRING>] [--type <TYPE>] [--market <STRING>] [--exchange <MIC>] \
                           [--cusip <CODE>] [--cik <KEY>] [--date <DATE>] [--search <TERM>] \
                           [--inactive] [--desc] [--sort <FIELD>] [--limit <INT>]
```

| Argument | Description | Required | Default |
|---|---|---:|---|
| --ticker | Filter by ticker symbol | No | All |
| --type | Filter by ticker type, see [Massvie: ticker types](https://massive.com/docs/rest/stocks/tickers/ticker-types) | No | All |
| --market | Filter by market type | No | All markets |
| --exchange | Primary exchange MIC (ISO 10383) | No | All exchanges |
| --cusip | CUSIP code filter | No | None |
| --cik | Central Index Key filter | No | None |
| --date | Point-in-time snapshot (YYYY-MM-DD) | No | Most recent |
| --search | Search within ticker/company name | No | None |
| --active-only | Include inactive tickers | No | Active only |
| --desc | Sort descending | No | Ascending |
| --sort | Field to sort results by | No | Default |
| --limit | Number of results | No | 100 |

| Option | Type | Range |
|---|---|---|
| --limit | Integer | Min: 1, Max: 1000 |

**Examples:**

**List all active tickers (paginated)**
```bash
$ marksapi massive tickers --limit 500
```

**Search for specific company**
```bash
$ marksapi massive tickers --search "Apple Inc" --limit 50
```

**Find ticker by CIK**
```bash
$ marksapi massive tickers --cik 0000320193 --limit 10
```

**Historical snapshot**
```bash
$ marksapi massive tickers --date 2023-06-15 --limit 200
```

**Filter by exchange and market**
```bash
$ marksapi massive tickers --exchange XNYS --market stocks --limit 100
```

#### ticker-info

Retrieve comprehensive details for a single ticker supported by Massive.

**Usage:**
```bash
$ marksapi massive ticker-info <MARKET> <TICKER> [--date <DATE>]
```

| Argument | Description | Required | Default |
|---|---|---:|---|
| MARKET | Applicable market identifier | Yes | — |
| TICKER | Single ticker symbol | Yes | — |
| date | Snapshot date (YYYY-MM-DD) | No | Most recent |

**Examples:**

**Single ticker overview**
```bash
$ marksapi massive ticker-info stocks AAPL
```

**Specific date snapshot**
```bash
$ marksapi massive ticker-info stocks MSFT --date 2024-01-01
```

**Multiple tickers**
```bash
$ marksapi massive ticker-info stocks GOOGL,NASDAQ:AAPL,ARCA:TSLA
```

### Output Formats

All commands support output formatting via `--format` flag:

| Format | Description |
|---|---|
| json | Structured JSON response |
| csv | CSV table export |

```bash
$ marksapi massive aggregate-bar stocks AAPL --from 2024-01-01 --to 2024-01-31 --timespan day --format csv
```

All output is written as **utf-8**-encoded text.

<sub>[Contents](#contents)</sub>

## Troubleshooting

### Exit Codes

| Code | Meaning |
|---:|---|
| 0 | Success |
| 1 | General error |
| 2 | Invalid argument |
| 3 | Authentication failure |
| 4 | API rate limited |
| 5 | Network error |

### Rate Limiting

If you receive exit code 4, implement backoff or reduce request frequency. Check your plan limits at https://massive.com/dashboard.

### Date Validation

Ensure dates conform to Eastern Time (ET) requirements specified by the Massive API. Use ISO 8601 format (YYYY-MM-DD).

### Authorization

Verify your API key is set correctly and hasn't expired.

<sub>[Contents](#contents)</sub>

### Attribution / Acknowledgements

See [Third-party notices](./THIRD-PARTY-NOTICES.md).

<sub>[Contents](#contents)</sub>