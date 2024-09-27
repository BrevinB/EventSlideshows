# EventSlideShows
EventSlideShows is a project I developed for my girlfriends sister as she was preparing for a graduation party. 
I came up with the idea to develop an app where guests of the grad party (or any event) can scan a QR code to be prompted to upload pictures either from the event
or from the past memories. These pictures would then display on a slideshow setup on a TV during the event. Afterwards, the images will be downloaded and shared with
host of the event. 

# Technologies Used
* Blazor
* MudBlazor
* MSSQL
* Docker
* HTML
* CSS
* JavaScript

# Demo

# I'm Most Proud Of....
There are two things I'm proud of with this project. 1. I came up with the idea a week before the grad party so I was short on time and was able to get it done. I
also received a lot of compliments during the event and I've now used this project for birthday parties and a couple grad parties. 2. The biggest issue I came 
across witht this project was the media size being too large when uploading so it would only load half of the image. I spent quite a bit of time researching ways 
to overcome that issue. I ended up figuring out how to compress the image when uploading and storing that way when I displayed the image it would be the correct size and quality. 

This was also my first time learning about and utilizing Docker.

Here's some of the code:
```C#
private async Task formatImages(byte[] buffer)
    {
        ProgressPercentage = 75;
            var format = "image/jpeg";
            var imageData = $"data:{{format}};base64,{Convert.ToBase64String(buffer)}";
            StateHasChanged();    
            await Task.Delay(1);
            Image newImage = new Image()
            {
                PeopleId = selectedPerson.Id,
                Data = imageData
            };
            ProgressPercentage = 85;
            await _ImageData.SaveImage(newImage);
            
        await GetImages();
    }

private async Task GetImages()
    {
        images = await _ImageData.GetImages(selectedPerson);
    }

private void OnInputFileChange(InputFileChangeEventArgs e)
    {
        selectedFile = e.GetMultipleFiles()[0];
        ProgressPercentage = 0;
        IsUploadDisabled = true;

        if (selectedFile.Size > maxFileSize)
        {
            SetAlert("alert alert-danger", "oi oi-ban", $"File size exceeds the limit. Maximum allowed size is <strong>{maxFileSize / (1024 * 1024)} MB</strong>.");
            return;
        }

        if (!allowedExtensions.Contains(Path.GetExtension(selectedFile.Name).ToLowerInvariant()))
        {
            SetAlert("alert alert-danger", "oi oi-warning", $"Invalid file type. Allowed file types are <strong>{string.Join(", ", allowedExtensions)}</strong>.");
            return;
        }

        SetAlert("alert alert-info", "oi oi-info", $"<strong>{selectedFile.Name}</strong> ({selectedFile.Size} bytes) file selected.");
        IsUploadDisabled = false;
    }

private static byte[] GetBytes(Stream stream)
    {
        var bytes = new byte[stream.Length];
        stream.Seek(0, SeekOrigin.Begin);
        stream.ReadAsync(bytes, 0, bytes.Length);
        stream.Dispose();
        return bytes;
    }
    
private async void OnSubmit()
    {
        isStriped = true;
        isUploading = true;
        IsUploadDisabled = true;
        StateHasChanged();
        
        if (selectedFile != null)
        {
            String base64String = "";
            IsUploadDisabled = true;
            var path = $"image\\{selectedFile.Name}";

            // Set buffer size to 512 KB.
            int bufferSize = 512 * 1024;
            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(bufferSize);
            int bytesRead;
            long totalBytesRead = 0;
            long fileSize = selectedFile.Size;
            ProgressPercentage = 25;
            // Use a timer to update the UI every few hundred milliseconds.
            using var timer = new Timer(_ => InvokeAsync(() => StateHasChanged()));
            timer.Change(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));

            try
            {
                    ProgressPercentage = 25;
                    await using MemoryStream xs = new MemoryStream();
                    await selectedFile.OpenReadStream(maxAllowedSize: 2147483648).CopyToAsync(xs);
                    ProgressPercentage = 50;
                    byte[] someBytes = GetBytes(xs);
                    await formatImages(someBytes);

            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
            }

            // Stop the timer and update the UI with the final progress.
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            ProgressPercentage = 100;

            SetAlert("alert alert-success", "oi oi-check", $"<strong>{selectedFile.Name}</strong> ({selectedFile.Size} bytes) file uploaded on server.");
            await SendSignal();
            inputFileId = Guid.NewGuid();
            selectedFile = null;
            isUploading = false;
            isStriped = false;
            this.StateHasChanged();
        }
    }
```
# Future of the Project
For right now the project has come to an end as events are slowing down, however I would like to imporve the application and eventually get it fully hosted so that 
others can use it for their events without needing me at the event. 
