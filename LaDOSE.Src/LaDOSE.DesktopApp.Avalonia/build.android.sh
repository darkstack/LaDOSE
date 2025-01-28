#!/usr/bin/env sh

export ANDROID_HOME=/home/tom/src/android/
export PATH=$PATH:$ANDROID_HOME/build-tools/34.0.0:$ANDROID_HOME/platforms/android-34

dotnet build  LaDOSE.DesktopApp.Avalonia.csproj -p:TargetFramework=net6.0-android -p:AndroidSdkDirectory=$ANDROID_HOME/build-tools/34.0.0
 