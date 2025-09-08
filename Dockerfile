#This is the Base image for the Building stage 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# switch into Working Dir on Docker\

WORKDIR /src
#copy the .sln/

COPY AcademySpacesAPI.sln ./
# Copy csproj file 

COPY Core/Core.csproj ./Core/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY WebAPI/WebAPI.csproj ./WebAPI/

#Restore
RUN dotnet restore 

COPY . . 

#publish the webAPI project 
WORKDIR /src/WebAPI
RUN dotnet publish -c Release -o /app/publish




#Get an Runtime image for dotent 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#Go into new working dir for docker 
WORKDIR /app
#copy over the from the build 
COPY --from=build /app/publish .
#set the base port 

ENV ASPNETCORE_URLS=http://0.0.0.0:7140
EXPOSE 7140
#expose the port 
#Add entry pionts 

ENTRYPOINT ["dotnet" , "WebAPI.dll"]

