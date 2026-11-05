# Moves the samples to the newest packages on nuget.org, builds them and pushes the result. Any failure
# stops it with nothing committed: a new VpnHood often needs the samples' code to change, which is then
# done by hand, on the packages this leaves in the working tree.
$ErrorActionPreference = "Stop";

function Assert-Success([string]$step)
{
	if ($LASTEXITCODE -ne 0) { throw "$step failed (exit $LASTEXITCODE)."; }
}

Write-Host
Write-Host "*** Updating Samples" -BackgroundColor Blue;

try
{
	Push-Location -Path $PSScriptRoot;

	# the commit holds what this update changed, so it starts from a clean tree
	if (git status --porcelain) { throw "The samples' working tree has changes. Commit them first."; }
	git pull --ff-only;
	Assert-Success "git pull";

	# Update nugets. The upgrade's own exit code tells nothing: on a head of two target frameworks it
	# fails on the second after the first has moved the version. What is left to upgrade is the answer.
	dotnet-outdated -u;
	dotnet-outdated --fail-on-updates;
	Assert-Success "The package update";

	if (-not (git status --porcelain)) { Write-Host "The samples are up to date."; return; }

	# build
	dotnet build;
	Assert-Success "The build";

	# commit and push
	$version = (Select-String -Path Directory.Build.props -Pattern "<VhPackageVersion>(.+)</VhPackageVersion>").Matches[0].Groups[1].Value;
	git add -- Directory.Build.props "*.csproj";
	Assert-Success "git add";
	git commit -m "Samples on the newest packages, VpnHood $version";
	Assert-Success "git commit";
	git push;
	Assert-Success "git push";
}
finally
{
	Pop-Location
}
