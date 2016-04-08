Param(
    [string]$sourceFileName, 
    [string]$nodeName, 
    [string]$nodeValue
    )

    #----------------------------------------------------------------------------
    # Modify the content of the node
    #----------------------------------------------------------------------------
    resolve-path $sourceFileName
        
        [xml]$configContent = New-Object XML
        $configContent.Load($sourceFileName)
        $propertyNodes = $configContent.GetElementsByTagName("add")
        foreach($node in $propertyNodes){if($node.GetAttribute("key") -eq $nodeName){$node.SetAttribute("value",$nodeValue)}}
        $configContent.save($sourceFileName)        
   