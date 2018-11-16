pushd %~dp0

regsvr32 /s MSCOMCTL.OCX

winmpq.exe d ../d2char.mpq (attributes)
winmpq.exe d ../d2data.mpq (attributes)
winmpq.exe d ../d2sfx.mpq (attributes)