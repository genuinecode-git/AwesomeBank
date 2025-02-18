#!/bin/bash

# Define the version of .NET to use (you can modify this based on your requirements)
DOTNET_VERSION="8.0"

# Function to check if .NET is installed
check_dotnet_installed() {
    if ! command -v dotnet &> /dev/null; then
        echo ".NET is not installed. Installing .NET SDK..."
        install_dotnet
    else
        echo ".NET is already installed."
    fi
}

# Function to install .NET SDK on Linux
install_dotnet() {
    # Instructions for Ubuntu/Debian
    wget https://packages.microsoft.com/config/ubuntu/22.04/prod.list
    sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
    sudo apt-get update
    sudo apt-get install -y dotnet-sdk-$DOTNET_VERSION
}

# Function to install any dependencies required for your app (if any)
install_dependencies() {
    echo "Checking for required dependencies..."

    # You can add additional dependencies here if required by your app
    # For example, installing runtime dependencies, packages, or libraries

    # Run a command to install any missing dependencies (if your app requires them)
    sudo apt-get install -y libicu-dev
}

# Function to run the application
run_application() {
    echo "Running App.."

    # Assuming your app's DLL is in the current directory (the publish folder)
    AwesomeBank.Console.dll
}

# Main script execution
check_dotnet_installed
install_dependencies
run_application
