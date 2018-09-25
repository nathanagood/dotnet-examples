# Steps to debug 

So, I followed the directions to download and run DynamoDB locally at from the
[AWS site](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.DownloadingAndRunning.html).
Specifically, I downloaded the `.tar.gz` file, extracted it, and ran the following command:

    $ java -Djava.library.path=./DynamoDBLocal_lib -jar DynamoDBLocal.jar -sharedDb