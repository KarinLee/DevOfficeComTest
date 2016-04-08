Param(
    [string]$nodeName,
    #If the follwoing is set as true, read the node from App.config, 
    #else, from AutoTestConfig.xml
    [bool]$fromAppConfig=$false
    )
 #   write-host $nodeName
#----------------------------------------------------------------------------
# Get the value of the node
#----------------------------------------------------------------------------       
[xml]$configContent = New-Object XML
if($fromAppConfig -eq $true)
{
    $configContent.Load("..\SearchEngineTest\App.config")
    $propertyNodes = $configContent.GetElementsByTagName("add")
}
else
{
    $configContent.Load(".\AutoTestConfig.xml")
    $propertyNodes = $configContent.GetElementsByTagName("Property")
}
foreach($node in $propertyNodes)
{
    if($node.GetAttribute("key") -eq $nodeName)
    {
        $value=$node.GetAttribute("value")
       #write-host $value
        return $value
    }
}