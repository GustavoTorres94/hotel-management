# :construction: README em construção ! :construction:

<h1 align="center"> Hotel Management :hotel: </h1>

<div align="center"> 
  
  ![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge)

</div>

<div align="center">
  <h3>Descrição</h3>
  <p>
    Uma aplicação back-end desenvolvida com ASP.NET e linguagem C#. Se trata de uma administração de reservas de quartos. Um CRUD que envolve os principais aspectos de uma
    administração hotoleira.<br>
    A aplicação possui JWT(Jason Web Token) para verificar as questões de autorização. SQL Server Dockerizado  e foi desenvolvida com o Entity Framework. <br>
    Os diretórios foram divididos em:<br>
    Controller: Onde está a lógica da aplicação.<br>
    Models: Representação do banco de Dados.<br>
    Repository: Lógicas de interação como banco de dados.<br>
    DTO: Entradas/Saídas das lógicas.<br>
  </p>
</div>

## Índice

- [Como Usar](#como-usar)
  
- [Funcionalidades](#funcionalidades)
  
- [Tecnologias](#tecnologias)

### Como Usar

    
<p>
  <ol>
    <li>
      Clone este repositório.

        git clone git@github.com:GustavoTorres94/hotel-management.git

  </li>
    <li>
      Entre no diretório criado.
    </li>
    <li>
      Entre no diretório src/Trybehotel e execute o comando para instalar as dependências do projeto.
      
    dotnet restore
                
  </li>
  <li>
    Na Raiz do projeto utilize o seguinte scrypt para "subir" os containers do Docker.

    docker compose up -d --build
        
  </li>
  <li>
    configure a string de conexção para acessar o banco de dados, caso não esteja configurada:

    var connectionString = "Server=localhost;Database=TrybeHotel;User=SA;Password=TrybeHotel12!;TrustServerCertificate=True";      
    
  </li>
  <li>
    Utilize a CLI para executar as migrations do Entity FrameWork

    dotnet ef database update

    
  </li>
  <li>
    Execute o CLI para rodar a aplicação.Utlize um software de auxílio (insomnia ou semelhante) ou uma extensão do vscode (thunderclient ou semelhante) para realizar as requisições para os endpoints. Com o projeto clonado podes fazer requisições para a rota 

      dotnet run
    
  </li>
  </ol>
</p>

### Funcionalidades

Endpoints:<br>
/booking - { post }<br>
/booking/:bookingId - { get }<br>
/city - { get, post, put }<br>
/geo/status - { get }<br>
/geo/address - { get }<br>
/hotel - { get, post }<br>
/login - { post }<br>
/room - { post } <br>
/room/:id { get, delete }<br>
/user { get, post }<br>

### /booking

Este endpoint contém serviços para lidar com operações relacionadas a reserva, A autenticação é feita utilizando JWT (JSON Web Tokens). Os usuários precisam estar autenticados para acessar os endpoints de reservas e devem possuir a política "Client".

Funcionalidades Principais
<ol>
  <li>POST|GET /booking </li>
  <ul>
    <li>
      Adiciona uma nova reserva.
    </li>
    <li>
      Autorização: Necessário token JWT com política "Client".
    </li>
    <li>
      POST - Corpo da requisição:

      {
        "RoomId": 1,
        "GuestQuant": 2,
        "CheckIn": "2023-06-01",
        "CheckOut": "2023-06-05"
      }
      
</li>
  <li>
    Possíveis respostas:
    <ul>
      <li>
        201 Created: Reserva criada com sucesso. POST
      </li>
      <li>
        400 Bad Request: Erro de validação ou sala não encontrada. POST
      </li>
      <li>
        200 OK: Detalhes da reserva retornados com sucesso. GET
      </li>
      <li>
        400 Bad Request: Erro ao buscar a reserva. GET
      </li>
    </ul>
  </li>
  </ul>
</ol>
  <br>
  
  ### /city

  Este endpoint contém as responsabilidades de tratar com as questões que envolvem as cidades, Como encontrar todas as cidades cadastradas e adicionar uma nova cidade.
  <ol>
    <li>
     GET /city
    </li>
    <ul>
      <li>
        Retorna a lista de todas as cidades.
      </li>
      <li>
        Resposta:
      </li>
      <ul>
        <li>
          200 OK: Lista de cidades.
        </li>
        <li>
          Exemplo de resposta:

          [
            {
              "CityId": 1,
              "Name": "São Paulo",
              "State": "SP"
            },
            {
              "CityId": 2,
              "Name": "Rio de Janeiro",
              "State": "RJ"
            }
          ]
          
</li>
      </ul>
    </ul>
    <li>
      POST /city
    </li>
    <ul>
      <li>
        Adiciona uma nova cidade.
      </li>
      <li>
        Corpo da requisição:

        {
          "Name": "Belo Horizonte",
          "State": "MG"
        }
        
  </li>
    <li>
      Possíveis respostas:
    </li>
    <ul>
      <li>
        201 Created: Cidade criada com sucesso.
      </li>
      <li>
        Exemplo de resposta:

        {
          "CityId": 3,
          "Name": "Belo Horizonte",
          "State": "MG"
        }
  </li>
    </ul>
    </ul>
    <li>
      PUT /city
    </li>
    <ul>
      <li>
        Atualiza uma cidade existente.
      </li>
      <li>
        Corpo da requisição:

        {
          "CityId": 3,
          "Name": "BH",
          "State": "MG"
        }
  </li>
    <li>
      Possíveis respostas:
    </li>
    <ul>
      <li>
        200 OK: Cidade atualizada com sucesso.
      </li>
      <li>
        400 Bad Request: Cidade não encontrada.
      </li>
      <li>
        Exemplo de resposta:

        {
          "CityId": 3,
          "Name": "BH",
          "State": "MG"
        }
   </li>
    </ul>
    </ul>
  </ol>
  <br>
  
  ###  /geo

  Este endpoint é responsável por lidar das questões de localização do usuário. Ele faz utilização de uma API externa que trás as localizações. 
  <ol>
    <li>
      GET /geo/status
    </li>
    <ul>
    <li>
      Ela verifica se o usuário correspondente ao ID decodificado do token existe no banco de dados e retorna o papel associado a esse usuário.<br>
    </li>
      <li>
        Retorna o status do serviço de geolocalização (Nominatim).
      </li>
      <li>
        Resposta:
      </li>
      <ul>
        <li>
          200 OK: Status do serviço.
        </li>
        <li>
          Exemplo de resposta:

          {
            "status": "OK",
            "message": "Service is running"
          }
  </li>
      </ul>
      <li>
        GET /geo/address
      </li>
      <ul>
        <li>
          Retorna a lista de hotéis baseados na localização fornecida.
        </li>
        <li>
          Corpo da requisição:

          {
            "Address": "Rua Exemplo",
            "City": "Cidade Exemplo",
            "State": "Estado Exemplo"
          }
  </li>
        <li>
          Possíveis respostas:
        </li>
        <ul>
          <li>
            200 OK: Lista de hotéis próximos à localização fornecida.
          </li>
          <li>
            400 Bad Request: Erro ao buscar a localização ou hotéis.
          </li>
          <li>
            Exemplo de resposta:

            [
              {
                "HotelId": 1,
                "Name": "Hotel Exemplo",
                "Address": "Rua Exemplo",
                "CityName": "Cidade Exemplo",
                "State": "Estado Exemplo",
                "Distance": 5
              },
              {
                "HotelId": 2,
                "Name": "Hotel Exemplo 2",
                "Address": "Rua Exemplo 2",
                "CityName": "Cidade Exemplo",
                "State": "Estado Exemplo",
                "Distance": 10
              }
            ]
  </li>
        </ul>
        </ul>
    </ul>
  </ol>
  
  ### /hotel

  Este endpoint é responsável pelas requisições relacionadas aos hotéis. A autenticação é feita através do JWT, algumas das funcionalidades utilizam a política de "Admin".
  <ol>
    <li>GET /hotel</li>
    <ul>
      <li>
        Retorna a lista de todos os hotéis.
      </li>
      <li>
        Resposta:
      </li>
      <ul>
        <li>200 OK: Lista de hotéis.</li>
        <li>500 Internal Server Error: Erro interno do servidor.</li>
        <li>
          Exemplo de resposta:

          [
            {
              "HotelId": 1,
              "Name": "Hotel Exemplo",
              "Address": "Rua Exemplo, 123",
              "CityId": 1,
              "CityName": "Cidade Exemplo",
              "State": "Estado Exemplo"
            },
            {
              "HotelId": 2,
              "Name": "Hotel Exemplo 2",
              "Address": "Rua Exemplo, 456",
              "CityId": 2,
              "CityName": "Outra Cidade",
              "State": "Outro Estado"
            }
          ]
  </li>
      </ul>
    </ul>
    <li>POST /hotel</li>
    <ul>
      <li>Adiciona um novo hotel. Requer autenticação JWT e permissão de administrador.</li>
      <li>
        Corpo da requisição:

        {
          "Name": "Hotel Novo",
          "Address": "Rua Nova, 789",
          "CityId": 3
        }
  </li>
      <li>Resposta:</li>
      <ul>
        <li>201 Created: Hotel criado com sucesso.</li>
        <li>500 Internal Server Error: Erro interno do servidor.</li>
        <li>
          Exemplo de resposta:

          {
            "HotelId": 3,
            "Name": "Hotel Novo",
            "Address": "Rua Nova, 789",
            "CityId": 3,
            "CityName": "Nova Cidade",
            "State": "Novo Estado"
          }
  </li>
      </ul>
    </ul>
  </ol>
</ol>

### /login

Este endpoint contém um serviço para lidar com Login de usuários e pequenas verificações de dados.

Funcionalidades Principais
<ol>
  <li>POST /login</li>
  <ul>
    <li>
      Autentica um usuário e retorna um token JWT.
    </li>
    <li>
      Corpo da requisição:

      {
        "Email": "usuario@exemplo.com",
        "Password": "senha"
      }
  </li>
  <li>Resposta:</li>
  <ul>
    <li>
      200 OK: Token JWT gerado com sucesso.

      {
        "token": "jwt_token"
      }
</li>
  <li>
    401 Unauthorized: E-mail ou senha incorretos.

    {
      "message": "Incorrect e-mail or password"
    }
</li>
  </ul>
  </ul>
  <br>
</ol>
<br>

### /room

Este endpoint contém um serviço para lidar com operações relacionadas aos quartos de um hotel. Ela utiliza a verficação JWT com a política de "Admin", somente com essa verificação algumas alterações são permitidas.

<ol>
  <li>GET /room/{HotelId}</li>
  <ul>
    <li>
      Retorna a lista de quartos de um hotel específico.
    </li>
    <li>
      Parâmetros:
    </li>
    <ul>
      <li>
        HotelId: ID do hotel.
      </li>
    </ul>
    <li>Resposta:</li>
    <ul>
      <li>
        200 OK: Lista de quartos do hotel.

        [
          {
            "RoomId": 1,
            "Name": "Quarto Luxo",
            "Capacity": 2,
            "Image": "url_da_imagem",
            "Hotel": {
              "HotelId": 1,
              "Name": "Hotel Exemplo",
              "Address": "Rua Exemplo, 123",
              "CityId": 1,
              "CityName": "Cidade Exemplo",
              "State": "Estado Exemplo"
            }
          }
        ]
  </li>
    </ul>
  </ul>
        <br>
<li>POST /room</li>
  <ul>
    <li>
      Adiciona um novo quarto.
    </li>
    <li>
      Autorização: Necessário token JWT e permissões de Admin.
    </li>
    <li>
      Corpo da requisição:

      {
        "Name": "Quarto Luxo",
        "Capacity": 2,
        "Image": "url_da_imagem",
        "HotelId": 1
      }
</li>
  <li>Resposta:</li>
  <ul>
    <li>
      201 Created: Quarto adicionado com sucesso.

      {
        "RoomId": 1,
        "Name": "Quarto Luxo",
        "Capacity": 2,
        "Image": "url_da_imagem",
        "Hotel": {
          "HotelId": 1,
          "Name": "Hotel Exemplo",
          "Address": "Rua Exemplo, 123",
          "CityId": 1,
          "CityName": "Cidade Exemplo",
          "State": "Estado Exemplo"
        }
      }
  </li>
  <li>400 Bad Request: Objeto de quarto inválido.</li>
  <li>500 Internal Server Error: Erro interno do servidor.</li>
  </ul>
  </ul>
        <br>
</ol>

### /user

O endpoint de user oferece funcionalidades para lidar com informações sobre usuários, desde cadastro até pesquisa por algum ou todos. A rota é verificada por autorizações JWT, pelo política de "Admin".


<ol>
  <li>GET /user</li>
  <ul>
    <li>
      Retorna a lista de usuários.
    </li>
    <li>
      Autorização: Necessário token JWT e permissões de Admin.
    </li>
    <li>Resposta:</li>
    <ul>
      <li>
        200 OK: Lista de usuários.

        [
          {
            "UserId": 1,
            "Name": "John Doe",
            "Email": "john.doe@example.com",
            "UserType": "admin"
          }
        ]
  </li>
    </ul>
  </ul>
        <br>
  <li>POST /user</li>
  <ul>
    <li>
      Adiciona um novo usuário.
    </li>
    <li>
      Corpo da requisição:

      {
        "Name": "John Doe",
        "Email": "john.doe@example.com",
        "Password": "password"
      }
 </li>
 <li>
   Resposta:
 </li>
 <ul>
   <li>
    201 Created: Usuário adicionado com sucesso.

    {
      "UserId": 1,
      "Name": "John Doe",
      "Email": "john.doe@example.com",
      "UserType": "client"
    }
   </li>
   <li>
     409 Conflict: Email do usuário já existe.

     {
      "message": "User email already exists"
     }
   </li>
 </ul>
  </ul>
<br>

### Tecnologias

Neste projeto utilizei as seguintes ferramentas:
<div align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white" />
  <img src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" />
</div>
