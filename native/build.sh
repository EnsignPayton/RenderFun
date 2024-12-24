#!/bin/bash

# Detect operating system
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    echo "Compiling on Linux..."
    gcc -shared -o clay.so -fPIC clay.c
elif [[ "$OSTYPE" == "msys"* || "$OSTYPE" == "cygwin"* || "$OSTYPE" == "win32" ]]; then
    echo "Compiling on Windows..."
    clang -shared -o clay.dll -DWIN32 clay.c
else
    echo "Unsupported OS: $OSTYPE"
    exit 1
fi

echo "Compilation complete."
