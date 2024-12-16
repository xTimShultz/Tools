# Wrap c# files in compiler flag - recurses subdiretory with prompt for each file
# Define the folder containing the C# files
$folderPath = "C:\Path\To\Your\Folder"

# Define the conditional compilation flag
$condition = "ENABLE_FLAG"

# Get all .cs files in the folder and subfolders
$files = Get-ChildItem -Path $folderPath -Filter "*.cs" -Recurse

# Process each file
foreach ($file in $files) {
    # Display the file name and prompt for confirmation
    Write-Host "File: $($file.FullName)" -ForegroundColor Yellow
    $response = Read-Host "Do you want to wrap this file with #if $condition? (y/n)"

    if ($response -ne "y") {
        Write-Host "Skipping: $($file.FullName)" -ForegroundColor Cyan
        continue
    }

    # Read the current file content
    $content = Get-Content -Path $file.FullName

    # Skip files already wrapped in the conditional flag
    if ($content[0] -match "^#if\s+$condition") {
        Write-Host "Already wrapped: $($file.FullName)" -ForegroundColor Cyan
        continue
    }

    # Create new content with the conditional compilation flags
    $newContent = @"
#if $condition
$(Get-Content $file.FullName)
#endif
"@

    # Write the updated content back to the file
    $newContent | Set-Content -Path $file.FullName

    Write-Host "Updated: $($file.FullName)" -ForegroundColor Green
}
