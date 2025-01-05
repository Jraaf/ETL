To use this code with much larger data sources and keep it working fast indexing would be great.
I have included one index just as an example beacuse there are only 43 out of 30000 vlues are passed into database.
All dates are parsed into UTC.

# note at 23:17 05/01/2025
i incorrectly understood the question about 10gb file,
to use it with larger csv files, it would be better to split the file into 20mb sections (let's say 50000 lines) and insert them sequentially. for that it would be better to create trigger in database so it would retrun us all dublicates to be inserted into dublicates.csv.
