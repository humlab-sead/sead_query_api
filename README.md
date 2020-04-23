# SEAD Query API

Backend for Supe

# Development

## User Linux dotnet install

Fetch Linux x64 binaries from ```https://dotnet.microsoft.com/download/dotnet-core```

```bash

mkdir -p $HOME/bin/dotnet && cd ~${HOME}/bin/dotnet

wget https://download.visualstudio.microsoft.com/download/pr/daec2daf-b458-4ae1-9046-b8ba09b5fb49/733e2d73b41640d6e6bdf1cc6b9ef03b/dotnet-sdk-3.1.200-linux-x64.tar.gz

tar zxf dotnet-sdk-3.1.200-linux-x64.tar.gz

export DOTNET_ROOT=$HOME/bin/dotnet
export PATH=$PATH:$HOME/bin/dotnet

```

## VS Code SSH Remote Development

 - Insstall OpenSSH

 - Generate keys:

 > ssh-keygen

 - Copy keys:

 > SET REMOTEHOST=user@your.remote.host
 > scp %USERPROFILE%\.ssh\id_rsa.pub %REMOTEHOST%:~/tmp.pub
 > ssh %REMOTEHOST% "mkdir -p ~/.ssh && chmod 700 ~/.ssh && cat ~/tmp.pub >> ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys && rm -f ~/tmp.pub"


C# vscode extension must be installed on remote host
