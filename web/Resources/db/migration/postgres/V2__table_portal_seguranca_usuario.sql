-- CRIANDO TABELA USUÁRIO
CREATE TABLE portal_seguranca.usuario
(
  id serial NOT NULL,
  nome character varying(50) NOT NULL,
  login character varying(50) NOT NULL,
  email character varying(50) NOT NULL,
  senha_hash bytea NOT NULL,
  senha_salt bytea NOT NULL,
  CONSTRAINT pk_usuario PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);

ALTER TABLE portal_seguranca.usuario OWNER TO postgres;

COMMENT ON TABLE portal_seguranca.usuario IS 'Entidade responsável por armazenar os usuários.';
COMMENT ON COLUMN portal_seguranca.usuario.id IS 'Identificador único da entidade.';
COMMENT ON COLUMN portal_seguranca.usuario.nome IS 'Nome do usuário.';
COMMENT ON COLUMN portal_seguranca.usuario.login IS 'Login do usuário.';
COMMENT ON COLUMN portal_seguranca.usuario.email IS 'Email do usuário.';
COMMENT ON COLUMN portal_seguranca.usuario.senha_hash IS 'Hash senha do usuário.';
COMMENT ON COLUMN portal_seguranca.usuario.senha_salt IS 'Salt senha do usuário.';

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE portal_seguranca.usuario TO gt4w_db;
GRANT SELECT, USAGE ON SEQUENCE portal_seguranca.usuario_id_seq TO gt4w_db;



--CRIANDO TABELA PERFIL
CREATE TABLE portal_seguranca.perfil
(
  id serial NOT NULL,
  codigo character varying(45) NOT NULL,
  nome character varying(200) NOT NULL,
  CONSTRAINT pk_perfil PRIMARY KEY (id)
)
WITH (
OIDS=FALSE
);

ALTER TABLE portal_seguranca.perfil OWNER TO postgres;


COMMENT ON TABLE portal_seguranca.perfil IS 'Entidade perfil do usuário.';
COMMENT ON COLUMN portal_seguranca.perfil.id IS 'Identificador único da entidade.';
COMMENT ON COLUMN portal_seguranca.perfil.codigo IS 'Código do perfil.';
COMMENT ON COLUMN portal_seguranca.perfil.nome IS 'Nome do perfil.';

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE portal_seguranca.perfil TO gt4w_db;
GRANT SELECT, USAGE ON SEQUENCE portal_seguranca.perfil_id_seq TO gt4w_db;



--CRIANDO TABELA USUÁRIO_PERFIL
CREATE TABLE portal_seguranca.rel_usuario_perfil
(
id_usuario integer NOT NULL,
id_perfil integer NOT NULL,
CONSTRAINT pk_rel_usuario_perfil PRIMARY KEY (id_usuario, id_perfil),
CONSTRAINT fk_rup_perfil FOREIGN KEY (id_perfil)
REFERENCES portal_seguranca.perfil (id) MATCH SIMPLE  ON UPDATE NO ACTION ON DELETE NO ACTION,
CONSTRAINT fk_rup_usuario FOREIGN KEY (id_usuario)
REFERENCES portal_seguranca.usuario (id) MATCH SIMPLE  ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
OIDS=FALSE
);

ALTER TABLE portal_seguranca.rel_usuario_perfil OWNER TO postgres;

COMMENT ON TABLE portal_seguranca.rel_usuario_perfil IS 'Entidade perfil e usuário.';
COMMENT ON COLUMN portal_seguranca.rel_usuario_perfil.id_usuario IS 'Identificador da entidade usuário.';
COMMENT ON COLUMN portal_seguranca.rel_usuario_perfil.id_perfil IS 'Identificador da entidade perfil.';

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE portal_seguranca.perfil TO gt4w_db;
GRANT SELECT, USAGE ON SEQUENCE portal_seguranca.perfil_id_seq TO gt4w_db;



