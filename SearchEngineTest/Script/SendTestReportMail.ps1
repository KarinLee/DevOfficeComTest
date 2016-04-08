param(
[String] $TestOutputFileLocation
)

function SubStringCount
{
    param(
    [string]$str,
    [string]$subStr
    )

    $count=0;    
    if ($str.Contains($subStr))
    {
        $count++;
        $strReplaced = $str.Replace($subStr, "");   
        return ($str.Length-$strReplaced.Length)/ $subStr.Length;            
    }
    return 0;
}

function ExtractKeyWordsAndRanking
{
    param(
    [string]$traceInfo,
    [int]$count
    )
    $searchedArr=@();
    $replacedString=$traceInfo
    
    for($i=0;$i -lt $count;$i++)
    {
        $keyWordsIndex=$replacedString.LastIndexOf("Keywords:")+9;
        $rankingIndex=$replacedString.LastIndexOf("Ranking:")+8;
        
        $searchedArr+=$replacedString.Substring($keyWordsIndex,$replacedString.LastIndexOf("Ranking:")-$keyWordsIndex)+"<->"+$replacedString.Substring($rankingIndex);
        $replacedString=$replacedString.Substring(0,$replacedString.LastIndexOf("Keywords:"));
    }
    for($j=0;$j -lt $searchedArr.Length/2;$j++)
    {
    $temp=$searchedArr[$j];
    $searchedArr[$j]=$searchedArr[$searchedArr.Length-$j-1];
    $searchedArr[$searchedArr.Length-$j-1]=$temp;
    }
    return $searchedArr;
}

if(-not $TestOutputFileLocation.EndsWith("\"))
{
$TestOutputFileLocation="$TestOutputFileLocation\";
}
resolve-path $TestOutputFileLocation;
Get-Location
#mail server configuration 
$smtpServer = .\GetConfigFileNode.ps1 "SMTPServerHost" 
$smtpUser = .\GetConfigFileNode.ps1 "SMTPUser"
$smtpPassword = .\GetConfigFileNode.ps1 "SMTPPassword"
$domain=.\GetConfigFileNode.ps1 "Domain"
$MailAddress =.\GetConfigFileNode.ps1 "FromAddress"
$toAddresses =(.\GetConfigFileNode.ps1 "ToAddresses").Split(";")
#create the mail message
$mail = New-Object System.Net.Mail.MailMessage

#set the addresses
$mail.From = New-Object System.Net.Mail.MailAddress($MailAddress)
foreach($toAddress in $toAddresses)
{
    $mail.To.Add($toAddress)
}
$mail.IsBodyHtml=$true
$date=Get-Date -Format 'yyyy-MM-dd-hh-mm-ss'
$mail.Subject = "[SeachEngine]Automation Test Report"+"_"+$date;
$mail.Body=""
$fileList=Get-ChildItem $TestOutputFileLocation;
foreach($tempFile in $fileList)
{
    #Construct the mail table
    $contents = Get-Content -Path $tempFile.FullName
    [xml]$traceContent = New-Object XML
    for($l=$contents.Length-1; $l -ge 0;$l--)
    {
        if($contents[$l].StartsWith("Results File:"))
        {
            $traceFileName=$contents[$l].Replace("Results File:","").TrimStart();
            $traceContent.Load($traceFileName);
            break;
        }      
    }
    $caseResultNodes = $traceContent.GetElementsByTagName("UnitTestResult")

    $mail.Body+='<table cellspacing="1" cellpadding="3" border="3" style="font-size: 12pt;line-height: 15px;border-left:3px;">';
    if($tempFile.BaseName.ToLower().Contains("google"))
    {
        $searchEngine="Google";
    }
    else
    {
        $searchEngine="Bing";
    }
    $mail.Body+="<tr style=`"background-color:#8B008B;color:#FFFFFF`"><th colspan=`"4`"style=`"text-align:center`">$searchEngine Search Results</th></tr>";
    $mail.Body+='<tr style="background-color:#8B008B;color:#FFFFFF"><th>Test Case Name</th><th>Search Key Words</th><th>Ranking</th><th>Test Result</th></tr>';
    $mail.Body+='<tr>';
    for($i=0;$i -lt $contents.Length; $i++)
    {
        if($contents[$i])
        {
            if($contents[$i].StartsWith("Fail"))
            {
                $mail.Priority = "High"
            }
            if($contents[$i].StartsWith("Failed") -or $contents[$i].StartsWith("Passed") -or $contents[$i].StartsWith("Inconclusive"))
            {
                $result=$contents[$i].Split(" ")[0]
                $searchCount=0;
                $searchedArr=@();
                #Add Case name
                $caseName=$contents[$i].SubString($result.Length).Trim()
                foreach($caseResultNode in $caseResultNodes)
                {
                    if($caseResultNode.GetAttribute("testName") -eq $caseName)
                    {
                        $traceInfo=$caseResultNode.GetElementsByTagName("StdOut")[0].InnerText;
                        $searchCount=SubStringCount $traceInfo "Keywords"
                        $searchedArr+=ExtractKeyWordsAndRanking $traceInfo $searchCount
                    }
                }
            
                $mail.Body+="<td";
                if($searchCount -gt 0)
                {
                    $mail.Body+=" rowspan=`"$searchCount`"";
                }           
                $mail.Body+=">";
                $mail.Body+=$caseName;
                $mail.Body+='</td>';
                        
                #Add Key Words and Ranking
                $mail.Body+='<td>';
                if($searchCount -gt 0)
                {
                    $mail.Body+=$searchedArr[0].Replace("<->",'</td><td>');
                }
                else
                {
                    $mail.Body+='</td><td>';
                }
                $mail.Body+='</td>';         
                #Add Result
                if($result -eq "Passed")
                {
                    $color="#008000";
                }
                elseif($result -eq "Failed")
                {
                    $color="#FF0000";
                }
                else
                {
                    $color="#F5DEB3";
                }
                $mail.Body+="<td";
                if($searchCount -gt 0)
                {
                $mail.Body+=" rowspan=`"$searchCount`"";
                }
                $mail.Body+="><p style=`"color:"+"$color`";>";
                $mail.Body+=$result;
                $mail.Body+='</p></td>';
                $mail.Body+='</tr>';
                        
                for($k=1;$k -lt $searchCount -and $searchCount -gt 0;$k++)
                {
                    $mail.Body+='<tr><td>'+$searchedArr[$k].Replace("<->",'</td><td>')+'</td></tr>';
                }           
            }
        }  
    }
    $mail.Body+='</table><br/>';
    #Add the trace file as attachment
    $attachment = new-Object System.Net.Mail.Attachment($traceFileName)
    $mail.Attachments.Add($attachment)
    $baseName=$traceFileName.SubString($traceFileName.LastIndexOf("\")+1);
    $mail.Body+="<div style=`"font-size: 12pt;`"> <br/>Trace file:$baseName</div><br/>";
}

$mail.Body+='<div style="font-size: 12pt;"> <br/>For details please see the attachment.</div>';


#Send the message
$smtpClient = New-Object System.Net.Mail.SmtpClient -argumentList $smtpServer
$smtpClient.Credentials = New-Object System.Net.NetworkCredential -argumentList $smtpUser,$smtpPassword,$domain
$smtpClient.EnableSsl = $true; 
$smtpClient.Timeout=60000;
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { return $true }
try { 
    $smtpClient.Send($mail) 
    write-host 'Test report mail is sent Successfully!' 
    } 

   catch { 
   write-host $_ 
   }