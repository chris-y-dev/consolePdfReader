# C# PDF Reader (Console application)

This is a basic C# console application that attempts to extract tables from a pdf file, and output data into CSV format. Follow these steps to try it out.

## Step 1: Prerequisites

Before you begin, make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)

## Step 2: Clone the Repository

Clone this repository to your local machine using Git:

```bash
git clone https://github.com/your-username/your-repo.git
```


## Step 3: Save pdf files into the correct directory

`\bin\Debug\net6.0`

## Step 4: Provide arguments to read/extract/write the files

Argument 1: PDF file name (e.g. document.pdf)

Argument 2: Output file name (e.g. myData.csv)

```bash
consolePdfReader.exe document.pdf myData.csv
```

## Step 5: Open CSV file to view output

Note: PDFs are non-structured and can be difficult to extract accurately. To our human eyes, PDFs can have structured data like tables. However, it is not structured - meaning it can be difficult for a program or a script to identify what is and isn't a table.

As a result, the output may fail to identify a table in your PDF file, or even mistakenly extract non-table data.