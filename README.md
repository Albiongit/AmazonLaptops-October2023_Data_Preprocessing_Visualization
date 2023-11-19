# AmazonLaptops-October2023_Data_Preprocessing_Visualization

## Overview
This project aims to pre-process and prepare the dataset (amazon_laptops.csv) for further analysis. The main objective is to handle data quality issues, perform integration, aggregation, sampling, cleaning, and implement strategies for handling null values. Additionally, the project involves dimension reduction, feature subset selection, feature creation, discretization, binarization, and transformation of the dataset.

## Project Structure
The project is implemented in C# and utilizes various helper classes and extensions for efficient data manipulation. The main file, Program.cs, orchestrates the data pre-processing steps.

## Dependencies
* Application.Converters: Contains converters for parsing and transforming data types.
* Application.Extensions: Includes extension methods for enhanced functionality.
* Application.Helpers: Provides helper methods for data processing.
* Application.Mappers: Contains mapping configurations for reading CSV data with correct model.
* SharedData.Data: Contains CSV files
* SharedData.Models: Defines the data models used in the project.

# Data Pre-processing Steps

## Data Loading and Exploration:
* The dataset is loaded from the CSV file (amazon_laptops.csv).
* The first 10 records are displayed for initial exploration and for preprocessed dataset.
* New preprocessed dataset is stored in a new csv file for further analysis and visualization.

## Null Value Analysis and Handling:
* Null values in each column are counted and displayed.
* Duplicates are checked and if any exists they will be removed.
* Unnecessary columns are removed, and null values are filled using specific strategies like filling with most repeated value (Ram and Hard disk columns), with average value (Screen size column) and removing the whole record if number of records with null value in that column is small (Price column).

## Data Summary:
* Number of records for original and preprocessed dataset.
* Null value counts are displayed for each column in original dataset
* Final null value counts are displayed for each column in preprocessed dataset.

# Conclusion
The pre-processing steps outlined in this project aim to ensure data quality and prepare the dataset for subsequent analysis. The implemented strategies for handling null values and data transformation contribute to a cleaner and more reliable dataset for further exploration and visualization.
