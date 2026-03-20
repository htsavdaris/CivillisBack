
CREATE PROCEDURE public.generatedashboardforuser(IN personid integer)
    LANGUAGE plpgsql
    AS $$
DECLARE
  v_fldtotalcases int;
  v_fldtotalopen int;
  v_fldtotalclosed int;
  v_fldtotalsuccess int;
  v_fldtotalfail int;
  
  v_fldmycases int;
  v_fldmycasesopen int;
  v_fldmycasesclosed int;
  v_fldmycasessuccess int;
  v_fldmycasesfail int;

  v_fldtotalcaseslm int;
  v_fldtotalopenlm int;
  v_fldtotalclosedlm int;
  v_fldtotalsuccesslm int;
  v_fldtotalfaillm int;
  
  v_fldmycaseslm int;
  v_fldmycasesopenlm int;
  v_fldmycasesclosedlm int;
  v_fldmycasessuccesslm int;
  v_fldmycasesfaillm int;
  
BEGIN
	
	SELECT count(fldcaseid)
	INTO v_fldtotalcases
	FROM public.tblcase;

	SELECT count(fldcaseid)
	INTO v_fldtotalcaseslm
	FROM public.tblcase
	WHERE fldcreatedat >= date_trunc('month', current_date - interval '1 month')
  	and fldcreatedat < date_trunc('month', current_date);
  
	SELECT count(fldcaseid)
	INTO v_fldmycases
	FROM public.tblcase
	WHERE fldmanagerid = personid;

	SELECT count(fldcaseid)
	INTO v_fldmycaseslm
	FROM public.tblcase
	WHERE fldmanagerid = personid
	and fldcreatedat >= date_trunc('month', current_date - interval '1 month')
  	and fldcreatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldtotalopen
  FROM public.tblcase
  WHERE fldcasestatusid = 0 ;

  SELECT count(fldcaseid)
  INTO v_fldtotalopenlm
  FROM public.tblcase
  WHERE fldcasestatusid = 0 
  and fldcreatedat >= date_trunc('month', current_date - interval '1 month')
  and fldcreatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldmycasesopen
  FROM public.tblcase
  WHERE fldcasestatusid = 0 and fldmanagerid = personid;

  SELECT count(fldcaseid)
  INTO v_fldmycasesopenlm
  FROM public.tblcase
  WHERE fldcasestatusid = 0 and fldmanagerid = personid
  and fldcreatedat >= date_trunc('month', current_date - interval '1 month')
  and fldcreatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldtotalclosed
  FROM public.tblcase
  WHERE fldcasestatusid > 0 ;

  SELECT count(fldcaseid)
  INTO v_fldtotalclosedlm
  FROM public.tblcase
  WHERE fldcasestatusid > 0
  and fldupdatedat >= date_trunc('month', current_date - interval '1 month')
  and fldupdatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldmycasesclosed
  FROM public.tblcase
  WHERE fldcasestatusid > 0 and fldmanagerid = personid;

  SELECT count(fldcaseid)
  INTO v_fldmycasesclosedlm
  FROM public.tblcase
  WHERE fldcasestatusid > 0 and fldmanagerid = personid
  and fldupdatedat >= date_trunc('month', current_date - interval '1 month')
  and fldupdatedat < date_trunc('month', current_date);
    
  SELECT count(fldcaseid)
  INTO v_fldtotalsuccess
  FROM public.tblcase
  WHERE fldcasestatusid = 1;

  SELECT count(fldcaseid)
  INTO v_fldtotalsuccesslm
  FROM public.tblcase
  WHERE fldcasestatusid = 1
  and fldupdatedat >= date_trunc('month', current_date - interval '1 month')
  and fldupdatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldmycasessuccess
  FROM public.tblcase
  WHERE  fldcasestatusid = 1 and fldmanagerid = personid;

  SELECT count(fldcaseid)
  INTO v_fldmycasessuccesslm
  FROM public.tblcase
  WHERE  fldcasestatusid = 1 and fldmanagerid = personid
  and fldupdatedat >= date_trunc('month', current_date - interval '1 month')
  and fldupdatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldtotalfail
  FROM public.tblcase
  WHERE fldcasestatusid = 2;

  SELECT count(fldcaseid)
  INTO v_fldtotalfaillm
  FROM public.tblcase
  WHERE fldcasestatusid = 2
  and fldupdatedat >= date_trunc('month', current_date - interval '1 month')
  and fldupdatedat < date_trunc('month', current_date);
  
  SELECT count(fldcaseid)
  INTO v_fldmycasesfail
  FROM public.tblcase
  WHERE  fldcasestatusid = 2 and fldmanagerid = personid;

  SELECT count(fldcaseid)
  INTO v_fldmycasesfaillm
  FROM public.tblcase
  WHERE  fldcasestatusid = 2 and fldmanagerid = personid
  and fldupdatedat >= date_trunc('month', current_date - interval '1 month')
  and fldupdatedat < date_trunc('month', current_date);
  
  
  DELETE FROM public.tbldashboard
  WHERE fldpersonid = personid;
  
  INSERT INTO public.tbldashboard (fldpersonid, fldtotalcases, fldtotalopen, fldtotalclosed, fldtotalsuccess, fldtotalfail, fldmycases, fldmycasesopen, fldmycasesclosed, fldmycasessuccess, fldmycasesfail,fldtotalcaseslm, fldtotalopenlm, fldtotalclosedlm, fldtotalsuccesslm, fldtotalfaillm, fldmycaseslm, fldmycasesopenlm, fldmycasesclosedlm, fldmycasessuccesslm, fldmycasesfaillm, fldcreatedby, fldupdatedby, fldcreatedat, fldupdatedat)
  VALUES (personid, v_fldtotalcases, v_fldtotalopen, v_fldtotalclosed, v_fldtotalsuccess, v_fldtotalfail, v_fldmycases, v_fldmycasesopen, v_fldmycasesclosed, v_fldmycasessuccess, v_fldmycasesfail,v_fldtotalcaseslm, v_fldtotalopenlm, v_fldtotalclosedlm, v_fldtotalsuccesslm, v_fldtotalfaillm, v_fldmycaseslm, v_fldmycasesopenlm, v_fldmycasesclosedlm, v_fldmycasessuccesslm, v_fldmycasesfaillm, 1, 1, current_timestamp, current_timestamp );
  
  
END;
$$;


ALTER PROCEDURE public.generatedashboardforuser(IN personid integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 16399)
-- Name: tblattribute; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblattribute (
    fldattributeid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fldattributedescription character varying(100) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblattribute OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 16402)
-- Name: tblAttribute_fldattributeid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."tblAttribute_fldattributeid_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."tblAttribute_fldattributeid_seq" OWNER TO postgres;

--
-- TOC entry 5159 (class 0 OID 0)
-- Dependencies: 216
-- Name: tblAttribute_fldattributeid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."tblAttribute_fldattributeid_seq" OWNED BY public.tblattribute.fldattributeid;


--
-- TOC entry 217 (class 1259 OID 16403)
-- Name: tblperson; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblperson (
    fldpersonid bigint NOT NULL,
    fldpersonlastname character varying(80) NOT NULL,
    fldpersonfirstname character varying(50),
    fldpersongender character varying(15),
    fldpersonprefix character varying(10),
    fldpersonstreet1 character varying(60),
    fldpersonnumber1 character varying(5),
    fldpersontk1 character varying(10),
    fldpersonmobile character varying(50),
    fldpersonemail character varying(255),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldprofession character varying(100),
    fldxorikidomiid character varying(18),
    flddatasource character varying(50),
    fldcomment text,
    fldcity character varying(50),
    fldofficevisit boolean DEFAULT false,
    fldcommunucationconsent boolean DEFAULT false,
    fldmobilizationconsent boolean DEFAULT false,
    fldisregistered boolean DEFAULT false
);


ALTER TABLE public.tblperson OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16412)
-- Name: tblEntity_fldentityd_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."tblEntity_fldentityd_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."tblEntity_fldentityd_seq" OWNER TO postgres;

--
-- TOC entry 5160 (class 0 OID 0)
-- Dependencies: 218
-- Name: tblEntity_fldentityd_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."tblEntity_fldentityd_seq" OWNED BY public.tblperson.fldpersonid;


--
-- TOC entry 219 (class 1259 OID 16413)
-- Name: tblcase; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcase (
    fldcaseid bigint NOT NULL,
    fldcasetitle text,
    fldcasetypeid bigint,
    fldcasedescription text,
    fldcaseopendate timestamp without time zone,
    fldcasestatusid bigint,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldcomments text,
    fldintermediateid bigint,
    fldintermediaterdetails text,
    fldbeneficiaryid bigint,
    fldbeneficiarydetails text,
    fldmanagerid bigint,
    fldconfidential boolean DEFAULT false,
    fldentityid bigint,
    fldentitycontactid bigint,
    fldreceptionid bigint
);


ALTER TABLE public.tblcase OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16419)
-- Name: tblcase_fldcaseid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblcase_fldcaseid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblcase_fldcaseid_seq OWNER TO postgres;

--
-- TOC entry 5161 (class 0 OID 0)
-- Dependencies: 220
-- Name: tblcase_fldcaseid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblcase_fldcaseid_seq OWNED BY public.tblcase.fldcaseid;


--
-- TOC entry 221 (class 1259 OID 16420)
-- Name: tblcaserow; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcaserow (
    fldcaserowid bigint NOT NULL,
    fldcaseid bigint,
    fldactiondesc character varying(255),
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldcreatedby bigint,
    fldupdatedby bigint
);


ALTER TABLE public.tblcaserow OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16423)
-- Name: tblcaserow_fldcaserowid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblcaserow_fldcaserowid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblcaserow_fldcaserowid_seq OWNER TO postgres;

--
-- TOC entry 5162 (class 0 OID 0)
-- Dependencies: 222
-- Name: tblcaserow_fldcaserowid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblcaserow_fldcaserowid_seq OWNED BY public.tblcaserow.fldcaserowid;


--
-- TOC entry 223 (class 1259 OID 16424)
-- Name: tblcasestatus; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcasestatus (
    fldcasestatusid bigint NOT NULL,
    fldcasestatusdesc character varying(50),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblcasestatus OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16427)
-- Name: tblcasestatus_fldcasestatusid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblcasestatus_fldcasestatusid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblcasestatus_fldcasestatusid_seq OWNER TO postgres;

--
-- TOC entry 5163 (class 0 OID 0)
-- Dependencies: 224
-- Name: tblcasestatus_fldcasestatusid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblcasestatus_fldcasestatusid_seq OWNED BY public.tblcasestatus.fldcasestatusid;


--
-- TOC entry 225 (class 1259 OID 16428)
-- Name: tblcasetype; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcasetype (
    fldcasetypeid bigint NOT NULL,
    fldcasetypedesc character varying(100) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblcasetype OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16431)
-- Name: tblcasetype_fldcasetypeid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblcasetype_fldcasetypeid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblcasetype_fldcasetypeid_seq OWNER TO postgres;

--
-- TOC entry 5164 (class 0 OID 0)
-- Dependencies: 226
-- Name: tblcasetype_fldcasetypeid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblcasetype_fldcasetypeid_seq OWNED BY public.tblcasetype.fldcasetypeid;


--
-- TOC entry 263 (class 1259 OID 16648)
-- Name: tbldashboard; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbldashboard (
    fldpersonid bigint NOT NULL,
    fldtotalcases bigint,
    fldtotalopen bigint,
    fldtotalclosed bigint,
    fldtotalsuccess bigint,
    fldtotalfail bigint,
    fldmycases bigint,
    fldmycasesopen bigint,
    fldmycasesclosed bigint,
    fldmycasessuccess bigint,
    fldmycasesfail bigint,
    fldtotalcaseslm bigint,
    fldtotalopenlm bigint,
    fldtotalclosedlm bigint,
    fldtotalsuccesslm bigint,
    fldtotalfaillm bigint,
    fldmycaseslm bigint,
    fldmycasesopenlm bigint,
    fldmycasesclosedlm bigint,
    fldmycasessuccesslm bigint,
    fldmycasesfaillm bigint,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tbldashboard OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 16435)
-- Name: tbldomitype; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbldomitype (
    fkddomitypeid bigint NOT NULL,
    fkddomitypedesc character varying(50) NOT NULL
);


ALTER TABLE public.tbldomitype OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 16438)
-- Name: tbleklogikidomi; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbleklogikidomi (
    flddomiid character varying(18) NOT NULL,
    fkddomitypeid bigint,
    fldperigrafi character varying(255),
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldcreatedby bigint,
    fldupdatedby bigint
);


ALTER TABLE public.tbleklogikidomi OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 16441)
-- Name: tblentity; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblentity (
    fldentityid bigint NOT NULL,
    fldentityname character varying(255) NOT NULL,
    fldentitytypeid bigint,
    fldaddress character varying(50),
    fldnumber character varying(8),
    fldtk character varying(10),
    fldcity character varying(50),
    fldcountry character varying(50),
    fldfixedphone character varying(50),
    fldmobile character varying(50),
    fldmail character varying(255),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblentity OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 16446)
-- Name: tblentity_fldentityid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblentity_fldentityid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblentity_fldentityid_seq OWNER TO postgres;

--
-- TOC entry 5165 (class 0 OID 0)
-- Dependencies: 230
-- Name: tblentity_fldentityid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblentity_fldentityid_seq OWNED BY public.tblentity.fldentityid;


--
-- TOC entry 231 (class 1259 OID 16447)
-- Name: tblentitytype; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblentitytype (
    fldentitytypeid bigint NOT NULL,
    fldentitytypedesc character varying(255) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblentitytype OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 16450)
-- Name: tblentitytype_fldentitytypeid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblentitytype_fldentitytypeid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblentitytype_fldentitytypeid_seq OWNER TO postgres;

--
-- TOC entry 5166 (class 0 OID 0)
-- Dependencies: 232
-- Name: tblentitytype_fldentitytypeid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblentitytype_fldentitytypeid_seq OWNED BY public.tblentitytype.fldentitytypeid;


--
-- TOC entry 233 (class 1259 OID 16451)
-- Name: tblimportedperson; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblimportedperson (
    fldpersonid bigint NOT NULL,
    fldpersonlastname character varying(80),
    fldpersonfirstname character varying(50),
    fldpersongender character varying(15),
    fldpersonprefix character varying(10),
    fldpersonstreet1 character varying(60),
    fldpersonnumber1 character varying(5),
    fldpersontk1 character varying(10),
    fldpersonmobile character varying(50),
    fldpersonemail character varying(255),
    fldcity character varying(50),
    fldprofession character varying(100),
    flddatasource character varying(50),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblimportedperson OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 16456)
-- Name: tblimportedperson_fldpersonid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblimportedperson_fldpersonid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblimportedperson_fldpersonid_seq OWNER TO postgres;

--
-- TOC entry 5167 (class 0 OID 0)
-- Dependencies: 234
-- Name: tblimportedperson_fldpersonid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblimportedperson_fldpersonid_seq OWNED BY public.tblimportedperson.fldpersonid;


--
-- TOC entry 235 (class 1259 OID 16457)
-- Name: tblinvitation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblinvitation (
    fldinvitationid bigint NOT NULL,
    fldtitle character varying(255),
    fldlocation character varying(255),
    fldhost character varying(255),
    fldentityid bigint,
    fldhostpersonid bigint,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldgopersonid bigint,
    fldcomments text,
    fldreplied bigint DEFAULT 0,
    flddatetime date
);


ALTER TABLE public.tblinvitation OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 16464)
-- Name: tblinvitation_fldinvitationid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblinvitation_fldinvitationid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblinvitation_fldinvitationid_seq OWNER TO postgres;

--
-- TOC entry 5168 (class 0 OID 0)
-- Dependencies: 236
-- Name: tblinvitation_fldinvitationid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblinvitation_fldinvitationid_seq OWNED BY public.tblinvitation.fldinvitationid;


--
-- TOC entry 264 (class 1259 OID 16670)
-- Name: tblinvitationreplies; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblinvitationreplies (
    fldreplyid bigint NOT NULL,
    fldreplydesc character varying(20)
);


ALTER TABLE public.tblinvitationreplies OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 16465)
-- Name: tbltask; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbltask (
    fldtaskid bigint NOT NULL,
    fldtitle character varying(255),
    fldimportance bigint,
    fldassignor bigint,
    fldassignee bigint,
    fldiscompleted boolean,
    fldtasksecuritygroup bigint,
    flddescription text,
    fldcreatedby bigint,
    fldupdatedby bigint,
    flddeadline timestamp without time zone,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tbltask OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 16470)
-- Name: tblpendingtask_fldpendingtaskid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblpendingtask_fldpendingtaskid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblpendingtask_fldpendingtaskid_seq OWNER TO postgres;

--
-- TOC entry 5169 (class 0 OID 0)
-- Dependencies: 238
-- Name: tblpendingtask_fldpendingtaskid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblpendingtask_fldpendingtaskid_seq OWNED BY public.tbltask.fldtaskid;


--
-- TOC entry 239 (class 1259 OID 16471)
-- Name: tblpersonattribute; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblpersonattribute (
    fldpersonattrid bigint NOT NULL,
    fldpersonid bigint NOT NULL,
    fldattributeid bigint NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblpersonattribute OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 16474)
-- Name: tblpersonattribute_fldpersonattrid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblpersonattribute_fldpersonattrid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblpersonattribute_fldpersonattrid_seq OWNER TO postgres;

--
-- TOC entry 5170 (class 0 OID 0)
-- Dependencies: 240
-- Name: tblpersonattribute_fldpersonattrid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblpersonattribute_fldpersonattrid_seq OWNED BY public.tblpersonattribute.fldpersonattrid;


--
-- TOC entry 241 (class 1259 OID 16475)
-- Name: tblpersonattributesaggregate; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblpersonattributesaggregate (
    fldpersonid bigint NOT NULL,
    fldaggattributesarr character varying(1024),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblpersonattributesaggregate OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 16480)
-- Name: tblprofession; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblprofession (
    fldprofessionid bigint NOT NULL,
    fldprofessionname character varying(50) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblprofession OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 16483)
-- Name: tblprofession_fldprofessionid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblprofession_fldprofessionid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblprofession_fldprofessionid_seq OWNER TO postgres;

--
-- TOC entry 5171 (class 0 OID 0)
-- Dependencies: 243
-- Name: tblprofession_fldprofessionid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblprofession_fldprofessionid_seq OWNED BY public.tblprofession.fldprofessionid;


--
-- TOC entry 244 (class 1259 OID 16484)
-- Name: tblrefreshtoken; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblrefreshtoken (
    flduserid bigint NOT NULL,
    fldissued timestamp without time zone,
    fldexpires timestamp without time zone,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldrefreshtoken uuid
);


ALTER TABLE public.tblrefreshtoken OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 16487)
-- Name: tblreport; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblreport (
    fldreportid bigint NOT NULL,
    fldreportname character varying(100),
    flduserid bigint NOT NULL,
    fldreportdate timestamp without time zone,
    fldfilename character varying(1024),
    fldbloburl character varying(1024),
    fldcreatedby bigint NOT NULL,
    fldupdatedby bigint NOT NULL,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblreport OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 16492)
-- Name: tblreport_fldcreatedby_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblreport_fldcreatedby_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblreport_fldcreatedby_seq OWNER TO postgres;

--
-- TOC entry 5172 (class 0 OID 0)
-- Dependencies: 246
-- Name: tblreport_fldcreatedby_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblreport_fldcreatedby_seq OWNED BY public.tblreport.fldcreatedby;


--
-- TOC entry 247 (class 1259 OID 16493)
-- Name: tblreport_fldreportid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblreport_fldreportid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblreport_fldreportid_seq OWNER TO postgres;

--
-- TOC entry 5173 (class 0 OID 0)
-- Dependencies: 247
-- Name: tblreport_fldreportid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblreport_fldreportid_seq OWNED BY public.tblreport.fldreportid;


--
-- TOC entry 248 (class 1259 OID 16494)
-- Name: tblreport_fldupdatedby_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblreport_fldupdatedby_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblreport_fldupdatedby_seq OWNER TO postgres;

--
-- TOC entry 5174 (class 0 OID 0)
-- Dependencies: 248
-- Name: tblreport_fldupdatedby_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblreport_fldupdatedby_seq OWNED BY public.tblreport.fldupdatedby;


--
-- TOC entry 249 (class 1259 OID 16495)
-- Name: tblreport_flduserid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblreport_flduserid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblreport_flduserid_seq OWNER TO postgres;

--
-- TOC entry 5175 (class 0 OID 0)
-- Dependencies: 249
-- Name: tblreport_flduserid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblreport_flduserid_seq OWNED BY public.tblreport.flduserid;


--
-- TOC entry 250 (class 1259 OID 16496)
-- Name: tblrole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblrole (
    fldroleid bigint NOT NULL,
    fldroledesc character varying(30) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblrole OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 16499)
-- Name: tblrole_fldroleid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblrole_fldroleid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblrole_fldroleid_seq OWNER TO postgres;

--
-- TOC entry 5176 (class 0 OID 0)
-- Dependencies: 251
-- Name: tblrole_fldroleid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblrole_fldroleid_seq OWNED BY public.tblrole.fldroleid;


--
-- TOC entry 252 (class 1259 OID 16500)
-- Name: tbluser; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbluser (
    flduserid bigint NOT NULL,
    fldlastname character varying(50),
    fldfirstname character varying(50),
    fldlogin character varying(30) NOT NULL,
    fldpassword character varying(255) NOT NULL,
    fldemail character varying(255) NOT NULL,
    fldmobile character varying(50),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldpersonid bigint
);


ALTER TABLE public.tbluser OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 16505)
-- Name: tbluser_flduserid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tbluser_flduserid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tbluser_flduserid_seq OWNER TO postgres;

--
-- TOC entry 5177 (class 0 OID 0)
-- Dependencies: 253
-- Name: tbluser_flduserid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tbluser_flduserid_seq OWNED BY public.tbluser.flduserid;


--
-- TOC entry 254 (class 1259 OID 16506)
-- Name: tbluserrole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbluserrole (
    flduserroleid bigint NOT NULL,
    flduserid bigint NOT NULL,
    fldroleid bigint NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tbluserrole OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 16509)
-- Name: tbluserrole_flduserroleid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tbluserrole_flduserroleid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tbluserrole_flduserroleid_seq OWNER TO postgres;

--
-- TOC entry 5178 (class 0 OID 0)
-- Dependencies: 255
-- Name: tbluserrole_flduserroleid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tbluserrole_flduserroleid_seq OWNED BY public.tbluserrole.flduserroleid;


--
-- TOC entry 266 (class 1259 OID 16700)
-- Name: vwcase; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwcase AS
 SELECT a.fldcaseid,
    a.fldcasetitle,
    a.fldcasetypeid,
    a.fldcasedescription,
    a.fldcaseopendate,
    a.fldcasestatusid,
    a.fldcomments,
    a.fldintermediateid,
    a.fldintermediaterdetails,
    a.fldbeneficiaryid,
    a.fldbeneficiarydetails,
    a.fldmanagerid,
    a.fldconfidential,
    a.fldentityid,
    a.fldentitycontactid,
    a.fldreceptionid,
    a.fldcreatedby,
    a.fldupdatedby,
    a.fldcreatedat,
    a.fldupdatedat,
    b.fldcasetypedesc,
    u.fldlastname,
    p1.fldpersonlastname AS fldbeneficiarylastname,
    p1.fldpersonfirstname AS fldbeneficiaryfirstname,
    p1.fldpersonmobile AS fldbeneficiarymobile,
    p2.fldpersonlastname AS fldintermediatelastname,
    p2.fldpersonfirstname AS fldintermediatefirstname,
    p2.fldpersonmobile AS fldintermediatemobile,
    p3.fldpersonlastname AS fldmanagerlastname,
    p3.fldpersonfirstname AS fldmanagerfirstname,
    p3.fldpersonmobile AS fldmanagermobile,
    p4.fldpersonlastname AS fldreceptionlastname,
    p4.fldpersonfirstname AS fldreceptionfirstname,
    p4.fldpersonmobile AS fldreceptionmobile,
    p5.fldpersonlastname AS fldentitycontactlastname,
    p5.fldpersonfirstname AS fldentitycontactfirstname,
    p5.fldpersonmobile AS fldentitycontactmobile,
    c.fldcasestatusdesc,
    e.fldentityname
   FROM (((((((((public.tblcase a
     LEFT JOIN public.tblcasetype b ON ((a.fldcasetypeid = b.fldcasetypeid)))
     LEFT JOIN public.tblcasestatus c ON ((a.fldcasestatusid = c.fldcasestatusid)))
     LEFT JOIN public.tbluser u ON ((a.fldcreatedby = u.flduserid)))
     LEFT JOIN public.tblperson p1 ON ((a.fldbeneficiaryid = p1.fldpersonid)))
     LEFT JOIN public.tblperson p2 ON ((a.fldintermediateid = p2.fldpersonid)))
     LEFT JOIN public.tblperson p3 ON ((a.fldmanagerid = p3.fldpersonid)))
     LEFT JOIN public.tblperson p4 ON ((a.fldreceptionid = p4.fldpersonid)))
     LEFT JOIN public.tblperson p5 ON ((a.fldentitycontactid = p5.fldpersonid)))
     LEFT JOIN public.tblentity e ON ((a.fldentityid = e.fldentityid)));


ALTER VIEW public.vwcase OWNER TO postgres;

--
-- TOC entry 256 (class 1259 OID 16516)
-- Name: vwcaserow; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwcaserow AS
 SELECT a.fldcaserowid,
    a.fldcaseid,
    a.fldactiondesc,
    a.fldcreatedat,
    a.fldupdatedat,
    a.fldcreatedby,
    a.fldupdatedby,
    u.fldlastname
   FROM (public.tblcaserow a
     JOIN public.tbluser u ON ((a.fldcreatedby = u.flduserid)));


ALTER VIEW public.vwcaserow OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 16520)
-- Name: vwentity; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwentity AS
 SELECT tblentity.fldentityid,
    tblentity.fldentityname,
    tblentity.fldentitytypeid,
    tblentity.fldaddress,
    tblentity.fldnumber,
    tblentity.fldtk,
    tblentity.fldcity,
    tblentity.fldcountry,
    tblentity.fldfixedphone,
    tblentity.fldmobile,
    tblentity.fldmail,
    tblentity.fldcreatedby AS createdby,
    tblentity.fldupdatedby AS updatedby,
    tblentity.fldcreatedat AS createdat,
    tblentity.fldupdatedat AS updatedat,
    tblentitytype.fldentitytypedesc
   FROM (public.tblentity
     JOIN public.tblentitytype ON ((tblentity.fldentitytypeid = tblentitytype.fldentitytypeid)));


ALTER VIEW public.vwentity OWNER TO postgres;

--
-- TOC entry 265 (class 1259 OID 16685)
-- Name: vwinvitation; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwinvitation AS
 SELECT a.fldinvitationid,
    a.fldtitle,
    a.fldlocation,
    a.fldhost,
    a.fldentityid,
    a.fldhostpersonid,
    a.fldreplied,
    a.fldcreatedby,
    a.fldupdatedby,
    a.fldcreatedat,
    a.fldupdatedat,
    a.fldgopersonid,
    a.fldcomments,
    a.flddatetime,
    i1.fldreplydesc,
    b1.fldpersonlastname AS fldgoeslastname,
    b1.fldpersonfirstname AS fldgoesfirstname,
    b1.fldpersonmobile AS fldgoesmobile,
    b2.fldpersonlastname AS fldorglastname,
    b2.fldpersonfirstname AS fldorgfirstname,
    b2.fldpersonmobile AS fldorgmobile,
    c.fldentityname,
    c.fldmobile AS fldentitymobile
   FROM ((((public.tblinvitation a
     LEFT JOIN public.tblinvitationreplies i1 ON ((a.fldreplied = i1.fldreplyid)))
     LEFT JOIN public.tblperson b1 ON ((a.fldgopersonid = b1.fldpersonid)))
     LEFT JOIN public.tblperson b2 ON ((a.fldhostpersonid = b2.fldpersonid)))
     LEFT JOIN public.tblentity c ON ((a.fldentityid = c.fldentityid)));


ALTER VIEW public.vwinvitation OWNER TO postgres;

--
-- TOC entry 258 (class 1259 OID 16530)
-- Name: vwpersonattrarr; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwpersonattrarr AS
 SELECT a.fldpersonid,
    a.fldpersonlastname,
    a.fldpersonfirstname,
    a.fldpersongender,
    a.fldpersonprefix,
    a.fldpersonstreet1,
    a.fldpersonnumber1,
    a.fldpersontk1,
    a.fldpersonmobile,
    a.fldpersonemail,
    a.fldprofession,
    a.fldcity,
    a.fldcreatedby,
    a.fldupdatedby,
    a.fldcreatedat,
    a.fldupdatedat,
    b.fldaggattributesarr
   FROM (public.tblperson a
     LEFT JOIN public.tblpersonattributesaggregate b ON ((a.fldpersonid = b.fldpersonid)));


ALTER VIEW public.vwpersonattrarr OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 16535)
-- Name: vwpersonattribute; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwpersonattribute AS
 SELECT b.fldpersonattrid,
    a.fldpersonid,
    a.fldpersonlastname,
    a.fldpersonfirstname,
    c.fldattributeid,
    c.fldattributedescription
   FROM ((public.tblperson a
     JOIN public.tblpersonattribute b ON ((a.fldpersonid = b.fldpersonid)))
     JOIN public.tblattribute c ON ((b.fldattributeid = c.fldattributeid)));


ALTER VIEW public.vwpersonattribute OWNER TO postgres;

--
-- TOC entry 260 (class 1259 OID 16540)
-- Name: vwpersonattributeall; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwpersonattributeall AS
 SELECT b.fldpersonattrid,
    a.fldpersonid,
    a.fldpersonprefix,
    a.fldpersonlastname,
    a.fldpersonfirstname,
    a.fldpersonmobile,
    a.fldpersonemail,
    a.fldpersonstreet1,
    a.fldpersonnumber1,
    a.fldpersontk1,
    a.fldcity,
    a.fldprofession,
    a.fldcreatedby,
    a.fldupdatedby,
    a.fldcreatedat,
    a.fldupdatedat,
    c.fldattributeid,
    c.fldattributedescription
   FROM ((public.tblperson a
     LEFT JOIN public.tblpersonattribute b ON ((a.fldpersonid = b.fldpersonid)))
     JOIN public.tblattribute c ON ((b.fldattributeid = c.fldattributeid)));


ALTER VIEW public.vwpersonattributeall OWNER TO postgres;

--
-- TOC entry 261 (class 1259 OID 16545)
-- Name: vwpersonattributesaggregate; Type: MATERIALIZED VIEW; Schema: public; Owner: postgres
--

CREATE MATERIALIZED VIEW public.vwpersonattributesaggregate AS
 SELECT fldpersonid,
    array_to_string(array_agg(fldattributedescription), ','::text) AS aggattributesarr
   FROM public.vwpersonattribute
  GROUP BY fldpersonid
  ORDER BY fldpersonid
  WITH NO DATA;


ALTER MATERIALIZED VIEW public.vwpersonattributesaggregate OWNER TO postgres;

--
-- TOC entry 262 (class 1259 OID 16551)
-- Name: vwuserrole; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vwuserrole AS
 SELECT a.flduserroleid,
    a.fldroleid,
    a.flduserid,
    b.fldlastname,
    b.fldfirstname,
    b.fldlogin,
    b.fldemail,
    b.fldmobile,
    c.fldroledesc,
    a.fldcreatedat,
    a.fldupdatedat,
    a.fldcreatedby,
    a.fldupdatedby
   FROM ((public.tbluserrole a
     JOIN public.tbluser b ON ((a.flduserid = b.flduserid)))
     JOIN public.tblrole c ON ((a.fldroleid = c.fldroleid)));


ALTER VIEW public.vwuserrole OWNER TO postgres;

--
-- TOC entry 4879 (class 2604 OID 16556)
-- Name: tblattribute fldattributeid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblattribute ALTER COLUMN fldattributeid SET DEFAULT nextval('public."tblAttribute_fldattributeid_seq"'::regclass);


--
-- TOC entry 4885 (class 2604 OID 16557)
-- Name: tblcase fldcaseid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblcase ALTER COLUMN fldcaseid SET DEFAULT nextval('public.tblcase_fldcaseid_seq'::regclass);


--
-- TOC entry 4887 (class 2604 OID 16558)
-- Name: tblcaserow fldcaserowid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblcaserow ALTER COLUMN fldcaserowid SET DEFAULT nextval('public.tblcaserow_fldcaserowid_seq'::regclass);


--
-- TOC entry 4888 (class 2604 OID 16559)
-- Name: tblcasestatus fldcasestatusid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblcasestatus ALTER COLUMN fldcasestatusid SET DEFAULT nextval('public.tblcasestatus_fldcasestatusid_seq'::regclass);


--
-- TOC entry 4889 (class 2604 OID 16560)
-- Name: tblcasetype fldcasetypeid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblcasetype ALTER COLUMN fldcasetypeid SET DEFAULT nextval('public.tblcasetype_fldcasetypeid_seq'::regclass);


--
-- TOC entry 4890 (class 2604 OID 16561)
-- Name: tblentity fldentityid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblentity ALTER COLUMN fldentityid SET DEFAULT nextval('public.tblentity_fldentityid_seq'::regclass);


--
-- TOC entry 4891 (class 2604 OID 16562)
-- Name: tblentitytype fldentitytypeid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblentitytype ALTER COLUMN fldentitytypeid SET DEFAULT nextval('public.tblentitytype_fldentitytypeid_seq'::regclass);


--
-- TOC entry 4892 (class 2604 OID 16563)
-- Name: tblimportedperson fldpersonid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblimportedperson ALTER COLUMN fldpersonid SET DEFAULT nextval('public.tblimportedperson_fldpersonid_seq'::regclass);


--
-- TOC entry 4893 (class 2604 OID 16564)
-- Name: tblinvitation fldinvitationid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblinvitation ALTER COLUMN fldinvitationid SET DEFAULT nextval('public.tblinvitation_fldinvitationid_seq'::regclass);


--
-- TOC entry 4880 (class 2604 OID 16565)
-- Name: tblperson fldpersonid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblperson ALTER COLUMN fldpersonid SET DEFAULT nextval('public."tblEntity_fldentityd_seq"'::regclass);


--
-- TOC entry 4896 (class 2604 OID 16566)
-- Name: tblpersonattribute fldpersonattrid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblpersonattribute ALTER COLUMN fldpersonattrid SET DEFAULT nextval('public.tblpersonattribute_fldpersonattrid_seq'::regclass);


--
-- TOC entry 4897 (class 2604 OID 16567)
-- Name: tblprofession fldprofessionid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblprofession ALTER COLUMN fldprofessionid SET DEFAULT nextval('public.tblprofession_fldprofessionid_seq'::regclass);


--
-- TOC entry 4898 (class 2604 OID 16568)
-- Name: tblreport fldreportid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblreport ALTER COLUMN fldreportid SET DEFAULT nextval('public.tblreport_fldreportid_seq'::regclass);


--
-- TOC entry 4899 (class 2604 OID 16569)
-- Name: tblreport flduserid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblreport ALTER COLUMN flduserid SET DEFAULT nextval('public.tblreport_flduserid_seq'::regclass);


--
-- TOC entry 4900 (class 2604 OID 16570)
-- Name: tblreport fldcreatedby; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblreport ALTER COLUMN fldcreatedby SET DEFAULT nextval('public.tblreport_fldcreatedby_seq'::regclass);


--
-- TOC entry 4901 (class 2604 OID 16571)
-- Name: tblreport fldupdatedby; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblreport ALTER COLUMN fldupdatedby SET DEFAULT nextval('public.tblreport_fldupdatedby_seq'::regclass);


--
-- TOC entry 4902 (class 2604 OID 16572)
-- Name: tblrole fldroleid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblrole ALTER COLUMN fldroleid SET DEFAULT nextval('public.tblrole_fldroleid_seq'::regclass);


--
-- TOC entry 4895 (class 2604 OID 16573)
-- Name: tbltask fldtaskid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tbltask ALTER COLUMN fldtaskid SET DEFAULT nextval('public.tblpendingtask_fldpendingtaskid_seq'::regclass);


--
-- TOC entry 4903 (class 2604 OID 16574)
-- Name: tbluser flduserid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tbluser ALTER COLUMN flduserid SET DEFAULT nextval('public.tbluser_flduserid_seq'::regclass);


--
-- TOC entry 4904 (class 2604 OID 16575)
-- Name: tbluserrole flduserroleid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tbluserrole ALTER COLUMN flduserroleid SET DEFAULT nextval('public.tbluserrole_flduserroleid_seq'::regclass);

