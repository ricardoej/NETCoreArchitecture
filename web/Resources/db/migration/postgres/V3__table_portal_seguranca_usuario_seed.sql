-- Usuário
INSERT INTO portal_seguranca.usuario(id, nome, email, login, senha_hash, senha_salt) VALUES (1, 'Administrador', 'admin@admin.com.br', 'admin', decode('893c75101a005aa1f397126490d9e50f192d6c56735f0a24d6c87261caa212e2497a278ece02387f5900127da3968389644fd0d9537db0d7e243816179b3a002', 'hex'), decode('8bbe5764beb08ce91966ddb45da294c809378b8e44539d29d728b36a32ab340fb45cb6f6da1bb7de1508ce7b67a4515ee8a40f1d1ae01cf919f8f8598244da668ae135fac4b5580edecf84c89c28a635063bada8a26b576ca0d3dc43b443cb71176b0d19b908b75d720fd563c8748a9b08cbf8d062034ef29ec3f3e27f269d26', 'hex'));

-- Perfil
INSERT INTO portal_seguranca.perfil(id, codigo, nome) VALUES (1, 'ADMIN', 'Administrador');

-- Perfil_Usuario
INSERT INTO portal_seguranca.rel_usuario_perfil(id_usuario, id_perfil) VALUES (1, 1);