# MaxxPro Documentation & Homepage

This directory contains the source code for the official documentation and homepage of **MaxxPro**, built using [Docusaurus](https://docusaurus.io/).

## Prerequisites

- [Node.js](https://nodejs.org/) v20.0 or higher
- npm

## Setup & Installation

```bash
cd docs
npm install
```

## Local Development

Start the local development server with hot-reload:

```bash
npm run start
```

By default, the site will be available at `http://localhost:3000/MaxxPro/`.

## Build

Generate the production static site for all configured locales (`ja-JP` and `en`):

```bash
npm run build
```

The output will be placed in `build/` (for Japanese default) and `build/en/` (for English).

## Testing the Production Build Locally

```bash
npm run serve
```

