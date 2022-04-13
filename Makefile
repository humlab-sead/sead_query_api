

release: test
	taskkill /IM "dotnet.exe" /F
	taskkill /IM "testhost.exe" /F

.PHONY: release test
