FROM mono
ARG configuration=Debug
WORKDIR /project
COPY . .
RUN nuget restore
RUN msbuild StanfordNLP.sln /t:Build /p:Configuration=${configuration}
CMD [ "mono", "StanfordNLP/bin/Release/StanfordNLP.exe" ]
