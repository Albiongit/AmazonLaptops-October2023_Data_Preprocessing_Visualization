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

# Outlier Detection and Removal

In this phase, robust outlier detection and removal techniques are applied to ensure the reliability of the dataset. Outliers and anomalies in key attributes are identified and addressed using statistical measures.

## Calculation of Mean and Standard Deviation:
* Mean and standard deviation are computed for each numerical column in the preprocessed dataset, including RAM, Hard Disk, Screen Size, and Price. These statistical measures serve as essential references, providing insights into the average value and variability of the data. Crucially, they form the basis for calculating Z-scores, aiding in the identification of outliers and anomalies within the dataset.

## Z-Score Calculation:
* Z-scores, indicative of how many standard deviations a data point is from the mean, are calculated for each numerical attribute. This allows for a standardized comparison across different attributes.

## Thresholds for Outlier Detection:
* Experimentally determined thresholds are set for each attribute (RAM, Hard Disk, Screen Size, and Price). These thresholds act as benchmarks for identifying potential outliers

## Identification of Outliers:
* Outliers are pinpointed by assessing the z-scores against the defined thresholds. This step provides insights into data points that deviate significantly from the expected distribution.

## Removal of Outliers:
* Outliers and anomalies are systematically removed from the dataset. The process involves updating the dataset by excluding records identified as outliers in one or more attributes.

## Updated Dataset Statistics:
* Post-outlier removal, the script reports the number of records in the preprocessed dataset. This step offers a glimpse into the impact of outlier removal on the overall dataset size.

# Correlation Matrix and General Statistics

After addressing outliers, the script proceeds with a comprehensive analysis of relationships and statistical summaries for key numerical columns.
## Correlation Matrix:
* A correlation matrix is generated to reveal the interplay between RAM, Hard Disk, Screen Size, and Price. This matrix aids in understanding potential dependencies between attributes.

## General Statistics:
* For each numerical column, detailed statistics are presented, including mean, standard deviation, median, minimum, maximum, and mode. These statistics offer a holistic view of the distribution and central tendencies of the dataset.

# Data Visualization

In this phase we display some data visualization starting from general bar charts for numerical column showing their frequency, static visualization by displaying Ram distribution via Pie chart and an interactive visualization by displaying laptops based on their Screen size value and their Price.

## Bar chart for Ram frequency
![RamFreq](https://github.com/Albiongit/AmazonLaptops-October2023_Data_Preprocessing_Visualization/assets/62037447/7e2f1cb4-8964-4cd6-a21e-9b1471b902b3)

## Bar chart for Hard disk frequency
![hardDiskFreq](https://github.com/Albiongit/AmazonLaptops-October2023_Data_Preprocessing_Visualization/assets/62037447/26a302d6-f83f-4b55-88cc-bf61b59f6fec)

## Bar chart for Screen size frequency
![screenFreq](https://github.com/Albiongit/AmazonLaptops-October2023_Data_Preprocessing_Visualization/assets/62037447/b973b2fe-aa63-434a-9627-67f2ddd7023d)

## Bar chart for Price frequency
![priceFreq](https://github.com/Albiongit/AmazonLaptops-October2023_Data_Preprocessing_Visualization/assets/62037447/a89a138f-6fb5-44d4-9b9d-dd2e6c65de0e)

## Static visualization - Pie chart for Ram distribution
![ramPie](https://github.com/Albiongit/AmazonLaptops-October2023_Data_Preprocessing_Visualization/assets/62037447/086e5eda-85d0-4e6e-b1e7-8422dd536117)

## Interactive visualization - Scatter chart for Laptops based on their screen size and price distribution
![screenVsPriceChart](https://github.com/Albiongit/AmazonLaptops-October2023_Data_Preprocessing_Visualization/assets/62037447/6244058b-b6df-4df4-9b99-098ebe549eb6)


# Conclusion
The comprehensive pre-processing steps undertaken in this project have been instrumental in fortifying the dataset for rigorous analysis. Through adept strategies for handling null values and meticulous data transformations, the dataset has undergone a transformation, emerging as a cleaner and more reliable foundation for in-depth exploration and visualization. These efforts not only contribute to enhanced data quality but also set the stage for robust analyses and insights in subsequent phases of the project. The project's commitment to addressing data quality issues ensures that the dataset is well-prepared to unveil meaningful patterns and trends, laying a solid groundwork for subsequent analytical endeavors.
