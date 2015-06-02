param($installPath, $toolsPath, $package, $project)

function Uninstall()
{
	Write-Host "Uninstalling " + $package.Id 
	uninstall-package $package.Id -ProjectName $project.Name
}

function RemoveFromPackageNode
{
	Write-Host "Removing from package node " + $package.Id 
	
	$projectDir = (Get-Item $project.FullName).Directory
	$packagesFile = $projectDir.FullName + "\packages.config"
	[xml]$xml = Get-Content $packagesFile
	$node = $xml.SelectSingleNode("/packages/package[@id='" + $package.Id + "']")
	[Void]$node.ParentNode.RemoveChild($node)
	$xml.Save($packagesFile)
}

Uninstall
RemoveFromPackageNode