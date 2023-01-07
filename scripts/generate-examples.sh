#!/bin/bash

echo "Generating markdown examples..."
./src/BicepDocs.Cli/bin/Debug/net6.0/bicepdocs generate filesystem \
    --folderPath docs/formatters/examples/inputs \
    --out ./docs/formatters/examples/generated-output/markdown \
    --config docs/example-config.yml \
    --formatter markdown

echo "Generating docusaurus examples..."
./src/BicepDocs.Cli/bin/Debug/net6.0/bicepdocs generate filesystem \
    --folderPath docs/formatters/examples/inputs \
    --out ./docs/formatters/examples/generated-output/docusaurus \
    --config docs/example-config.yml \
    --formatter docusaurus