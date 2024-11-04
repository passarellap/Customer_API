#!/bin/bash

trap 'catch' ERR
catch() {
  echo "An error adding Nuget Server !"
}

echo "Total arguments : $#"
echo "Arguments : $@"
for arg in "$@"
do
	eval "arr=($arg)"
	for item in "${arr[@]}"; do 
		echo "Adding Nuget Server = $item";
		dotnet nuget add source $item;
	done
done

exit 0