// generate-token.js
const jwt = require("jsonwebtoken");

const token = jwt.sign(
  {
    sub: "11111111-1111-1111-1111-111111111111",
    org_id: "22222222-2222-2222-2222-222222222222"
  },
  "f3a9c7d2a4b8e1c6f9a0d7e5c2b4a8e9",
  {
    issuer: "educrm",
    audience: "educrm-api",
    expiresIn: "1h"
  }
);

console.log(token);
