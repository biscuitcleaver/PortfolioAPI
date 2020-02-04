# Enigma Machine API
This repository is the REST API for my Enigma Machine Frontend. 

## Features
This project was designed to exemplify how I handle business level logic and return data to the end user
* Written in .net Core 2
* Endpoints validate request before processing business logic, and return data in a JSON format
* Core business logic abstracted using a dependency injection model. Opted to use Ninject due to familiarity but could have also proceeded with built in DI framework. 
* Endpoints are protected to development and production urls using CORS.
* All controllers bind to a data transfer object for security. 
* Deployed to Elastic Cloud Compute services via AWS Fargate
* Persistence written to AWS dynamoDB

## Status
This is an active project that I will continue to grow and build additional features out. If you'd like to see one added, please add a feature request in the Issues tab.
