$find = 'MelonLoader'
$replace = 'PotatoLoader'
$match = '*.cshtml' , '*.vbhtml'
$preview = $true

foreach ($sc in dir -recurse | where { test-path $_.fullname -pathtype leaf} ) {
    select-string -path $sc -pattern $find
    if (!$preview) {
       (get-content $sc) | foreach-object { $_ -replace $find, $replace } | set-content $sc
    }
}