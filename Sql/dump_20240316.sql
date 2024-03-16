--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: ladoseapi; Type: SCHEMA; Schema: -; Owner: tom
--

CREATE SCHEMA ladoseapi;


ALTER SCHEMA ladoseapi OWNER TO tom;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: applicationrole; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.applicationrole (
    id bigint NOT NULL,
    name character varying(50) NOT NULL
);


ALTER TABLE ladoseapi.applicationrole OWNER TO tom;

--
-- Name: applicationrole_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.applicationrole_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.applicationrole_id_seq OWNER TO tom;

--
-- Name: applicationrole_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.applicationrole_id_seq OWNED BY ladoseapi.applicationrole.id;


--
-- Name: applicationuser; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.applicationuser (
    id bigint NOT NULL,
    firstname character varying(45) DEFAULT NULL::character varying,
    lastname character varying(45) DEFAULT NULL::character varying,
    username character varying(45) DEFAULT NULL::character varying,
    passwordhash bytea,
    passwordsalt bytea
);


ALTER TABLE ladoseapi.applicationuser OWNER TO tom;

--
-- Name: applicationuser_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.applicationuser_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.applicationuser_id_seq OWNER TO tom;

--
-- Name: applicationuser_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.applicationuser_id_seq OWNED BY ladoseapi.applicationuser.id;


--
-- Name: applicationuserrole; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.applicationuserrole (
    userid bigint NOT NULL,
    roleid bigint NOT NULL
);


ALTER TABLE ladoseapi.applicationuserrole OWNER TO tom;

--
-- Name: botevent; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.botevent (
    id bigint NOT NULL,
    name character varying(50) NOT NULL,
    date date DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE ladoseapi.botevent OWNER TO tom;

--
-- Name: botevent_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.botevent_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.botevent_id_seq OWNER TO tom;

--
-- Name: botevent_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.botevent_id_seq OWNED BY ladoseapi.botevent.id;


--
-- Name: boteventresult; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.boteventresult (
    id bigint NOT NULL,
    boteventid bigint NOT NULL,
    name character varying(50) NOT NULL,
    discordid character varying(50) NOT NULL,
    result boolean NOT NULL
);


ALTER TABLE ladoseapi.boteventresult OWNER TO tom;

--
-- Name: boteventresult_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.boteventresult_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.boteventresult_id_seq OWNER TO tom;

--
-- Name: boteventresult_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.boteventresult_id_seq OWNED BY ladoseapi.boteventresult.id;


--
-- Name: challongeparticipent; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.challongeparticipent (
    id bigint NOT NULL,
    challongeid bigint DEFAULT '0'::bigint NOT NULL,
    challongetournamentid bigint DEFAULT '0'::bigint NOT NULL,
    name character varying(500) DEFAULT '0'::character varying NOT NULL,
    rank bigint DEFAULT '0'::bigint,
    ismember boolean
);


ALTER TABLE ladoseapi.challongeparticipent OWNER TO tom;

--
-- Name: challongeparticipent_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.challongeparticipent_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.challongeparticipent_id_seq OWNER TO tom;

--
-- Name: challongeparticipent_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.challongeparticipent_id_seq OWNED BY ladoseapi.challongeparticipent.id;


--
-- Name: challongetournament; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.challongetournament (
    id bigint NOT NULL,
    challongeid bigint DEFAULT '0'::bigint NOT NULL,
    name character varying(500) DEFAULT NULL::character varying,
    gameid bigint,
    url character varying(255) DEFAULT NULL::character varying,
    sync timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE ladoseapi.challongetournament OWNER TO tom;

--
-- Name: challongetournament_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.challongetournament_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.challongetournament_id_seq OWNER TO tom;

--
-- Name: challongetournament_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.challongetournament_id_seq OWNED BY ladoseapi.challongetournament.id;


--
-- Name: event; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.event (
    id bigint NOT NULL,
    name character varying(255) NOT NULL,
    date timestamp with time zone NOT NULL,
    smashid bigint,
    smashslug character varying(255) DEFAULT NULL::character varying
);


ALTER TABLE ladoseapi.event OWNER TO tom;

--
-- Name: event_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.event_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.event_id_seq OWNER TO tom;

--
-- Name: event_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.event_id_seq OWNED BY ladoseapi.event.id;


--
-- Name: game; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.game (
    id bigint NOT NULL,
    name character varying(45) DEFAULT NULL::character varying,
    imgurl character varying(255) DEFAULT NULL::character varying,
    wordpresstag character varying(255) DEFAULT NULL::character varying,
    wordpresstagos character varying(255) DEFAULT NULL::character varying,
    "order" bigint DEFAULT '0'::bigint NOT NULL,
    longname character varying(255) DEFAULT NULL::character varying,
    smashid bigint
);


ALTER TABLE ladoseapi.game OWNER TO tom;

--
-- Name: game_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.game_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.game_id_seq OWNER TO tom;

--
-- Name: game_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.game_id_seq OWNED BY ladoseapi.game.id;


--
-- Name: player; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.player (
    id bigint NOT NULL,
    name character varying(150) NOT NULL,
    challongeid bigint,
    smashid bigint,
    gamertag character varying(150) DEFAULT NULL::character varying
);


ALTER TABLE ladoseapi.player OWNER TO tom;

--
-- Name: player_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.player_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.player_id_seq OWNER TO tom;

--
-- Name: player_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.player_id_seq OWNED BY ladoseapi.player.id;


--
-- Name: result; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.result (
    id bigint NOT NULL,
    playerid bigint DEFAULT '0'::bigint NOT NULL,
    tournamentid bigint DEFAULT '0'::bigint NOT NULL,
    point bigint DEFAULT '0'::bigint NOT NULL,
    rank bigint DEFAULT '0'::bigint NOT NULL
);


ALTER TABLE ladoseapi.result OWNER TO tom;

--
-- Name: result_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.result_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.result_id_seq OWNER TO tom;

--
-- Name: result_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.result_id_seq OWNED BY ladoseapi.result.id;


--
-- Name: set; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.set (
    id bigint NOT NULL,
    tournamentid bigint NOT NULL,
    player1id bigint NOT NULL,
    player2id bigint NOT NULL,
    player1score bigint,
    player2score bigint,
    round bigint NOT NULL
);


ALTER TABLE ladoseapi.set OWNER TO tom;

--
-- Name: set_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.set_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.set_id_seq OWNER TO tom;

--
-- Name: set_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.set_id_seq OWNED BY ladoseapi.set.id;


--
-- Name: todo; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.todo (
    id bigint NOT NULL,
    "user" character varying(45) NOT NULL,
    task text,
    done smallint DEFAULT '0'::smallint NOT NULL,
    created timestamp with time zone NOT NULL,
    deleted timestamp with time zone
);


ALTER TABLE ladoseapi.todo OWNER TO tom;

--
-- Name: todo_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.todo_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.todo_id_seq OWNER TO tom;

--
-- Name: todo_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.todo_id_seq OWNED BY ladoseapi.todo.id;


--
-- Name: tournament; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.tournament (
    id bigint NOT NULL,
    name character varying(150) NOT NULL,
    smashid bigint,
    challongeid bigint,
    eventid bigint,
    gameid bigint,
    finish boolean NOT NULL
);


ALTER TABLE ladoseapi.tournament OWNER TO tom;

--
-- Name: tournament_id_seq; Type: SEQUENCE; Schema: ladoseapi; Owner: tom
--

CREATE SEQUENCE ladoseapi.tournament_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE ladoseapi.tournament_id_seq OWNER TO tom;

--
-- Name: tournament_id_seq; Type: SEQUENCE OWNED BY; Schema: ladoseapi; Owner: tom
--

ALTER SEQUENCE ladoseapi.tournament_id_seq OWNED BY ladoseapi.tournament.id;


--
-- Name: wpbooking; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.wpbooking (
    wpeventid bigint,
    wpuserid bigint,
    message text,
    meta text
);


ALTER TABLE ladoseapi.wpbooking OWNER TO tom;

--
-- Name: wpevent; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.wpevent (
    id bigint NOT NULL,
    name character varying(255) DEFAULT NULL::character varying,
    slug character varying(255) DEFAULT NULL::character varying,
    date date
);


ALTER TABLE ladoseapi.wpevent OWNER TO tom;

--
-- Name: wpuser; Type: TABLE; Schema: ladoseapi; Owner: tom
--

CREATE TABLE ladoseapi.wpuser (
    id bigint NOT NULL,
    name character varying(45) DEFAULT NULL::character varying,
    wpuserlogin character varying(45) DEFAULT NULL::character varying,
    wpmail character varying(45) DEFAULT NULL::character varying
);


ALTER TABLE ladoseapi.wpuser OWNER TO tom;

--
-- Name: applicationrole id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.applicationrole ALTER COLUMN id SET DEFAULT nextval('ladoseapi.applicationrole_id_seq'::regclass);


--
-- Name: applicationuser id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.applicationuser ALTER COLUMN id SET DEFAULT nextval('ladoseapi.applicationuser_id_seq'::regclass);


--
-- Name: botevent id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.botevent ALTER COLUMN id SET DEFAULT nextval('ladoseapi.botevent_id_seq'::regclass);


--
-- Name: boteventresult id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.boteventresult ALTER COLUMN id SET DEFAULT nextval('ladoseapi.boteventresult_id_seq'::regclass);


--
-- Name: challongeparticipent id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.challongeparticipent ALTER COLUMN id SET DEFAULT nextval('ladoseapi.challongeparticipent_id_seq'::regclass);


--
-- Name: challongetournament id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.challongetournament ALTER COLUMN id SET DEFAULT nextval('ladoseapi.challongetournament_id_seq'::regclass);


--
-- Name: event id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.event ALTER COLUMN id SET DEFAULT nextval('ladoseapi.event_id_seq'::regclass);


--
-- Name: game id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.game ALTER COLUMN id SET DEFAULT nextval('ladoseapi.game_id_seq'::regclass);


--
-- Name: player id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.player ALTER COLUMN id SET DEFAULT nextval('ladoseapi.player_id_seq'::regclass);


--
-- Name: result id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.result ALTER COLUMN id SET DEFAULT nextval('ladoseapi.result_id_seq'::regclass);


--
-- Name: set id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.set ALTER COLUMN id SET DEFAULT nextval('ladoseapi.set_id_seq'::regclass);


--
-- Name: todo id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.todo ALTER COLUMN id SET DEFAULT nextval('ladoseapi.todo_id_seq'::regclass);


--
-- Name: tournament id; Type: DEFAULT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.tournament ALTER COLUMN id SET DEFAULT nextval('ladoseapi.tournament_id_seq'::regclass);


--
-- Name: applicationrole idx_17720_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.applicationrole
    ADD CONSTRAINT idx_17720_primary PRIMARY KEY (id);


--
-- Name: applicationuser idx_17725_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.applicationuser
    ADD CONSTRAINT idx_17725_primary PRIMARY KEY (id);


--
-- Name: botevent idx_17738_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.botevent
    ADD CONSTRAINT idx_17738_primary PRIMARY KEY (id);


--
-- Name: boteventresult idx_17744_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.boteventresult
    ADD CONSTRAINT idx_17744_primary PRIMARY KEY (id);


--
-- Name: challongeparticipent idx_17749_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.challongeparticipent
    ADD CONSTRAINT idx_17749_primary PRIMARY KEY (id);


--
-- Name: challongetournament idx_17760_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.challongetournament
    ADD CONSTRAINT idx_17760_primary PRIMARY KEY (id);


--
-- Name: event idx_17771_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.event
    ADD CONSTRAINT idx_17771_primary PRIMARY KEY (id);


--
-- Name: game idx_17779_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.game
    ADD CONSTRAINT idx_17779_primary PRIMARY KEY (id);


--
-- Name: result idx_17798_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.result
    ADD CONSTRAINT idx_17798_primary PRIMARY KEY (id);


--
-- Name: set idx_17807_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.set
    ADD CONSTRAINT idx_17807_primary PRIMARY KEY (id);


--
-- Name: todo idx_17812_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.todo
    ADD CONSTRAINT idx_17812_primary PRIMARY KEY (id);


--
-- Name: tournament idx_17820_primary; Type: CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.tournament
    ADD CONSTRAINT idx_17820_primary PRIMARY KEY (id);


--
-- Name: idx_17734_fk_applicationuserrole_applicationrole; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE INDEX idx_17734_fk_applicationuserrole_applicationrole ON ladoseapi.applicationuserrole USING btree (roleid);


--
-- Name: idx_17734_userid_roleid; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE UNIQUE INDEX idx_17734_userid_roleid ON ladoseapi.applicationuserrole USING btree (userid, roleid);


--
-- Name: idx_17744_fk_botevent_botevent; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE INDEX idx_17744_fk_botevent_botevent ON ladoseapi.boteventresult USING btree (boteventid);


--
-- Name: idx_17760_challongetournament_gameidpk; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE INDEX idx_17760_challongetournament_gameidpk ON ladoseapi.challongetournament USING btree (gameid);


--
-- Name: idx_17779_name_unique; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE UNIQUE INDEX idx_17779_name_unique ON ladoseapi.game USING btree (name);


--
-- Name: idx_17792_id; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE INDEX idx_17792_id ON ladoseapi.player USING btree (id);


--
-- Name: idx_17820_uniq_smashid; Type: INDEX; Schema: ladoseapi; Owner: tom
--

CREATE UNIQUE INDEX idx_17820_uniq_smashid ON ladoseapi.tournament USING btree (smashid);


--
-- Name: challongetournament challongetournament_gameidpk; Type: FK CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.challongetournament
    ADD CONSTRAINT challongetournament_gameidpk FOREIGN KEY (gameid) REFERENCES ladoseapi.game(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: applicationuserrole fk_applicationuserrole_applicationrole; Type: FK CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.applicationuserrole
    ADD CONSTRAINT fk_applicationuserrole_applicationrole FOREIGN KEY (roleid) REFERENCES ladoseapi.applicationrole(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: applicationuserrole fk_applicationuserrole_applicationuser; Type: FK CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.applicationuserrole
    ADD CONSTRAINT fk_applicationuserrole_applicationuser FOREIGN KEY (userid) REFERENCES ladoseapi.applicationuser(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: boteventresult fk_botevent_botevent; Type: FK CONSTRAINT; Schema: ladoseapi; Owner: tom
--

ALTER TABLE ONLY ladoseapi.boteventresult
    ADD CONSTRAINT fk_botevent_botevent FOREIGN KEY (boteventid) REFERENCES ladoseapi.botevent(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- PostgreSQL database dump complete
--

