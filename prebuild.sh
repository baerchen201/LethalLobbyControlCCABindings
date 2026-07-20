#!/usr/bin/env bash
set -euo pipefail

mkdir -p lib

temp="$(mktemp -d)"

wget -O "${temp}/LobbyControl.zip" https://thunderstore.io/package/download/mattymatty/LobbyControl/2.5.12/
7z e -aoa "${temp}/LobbyControl.zip" -olib "BepInEx/plugins/LobbyControl.dll"
