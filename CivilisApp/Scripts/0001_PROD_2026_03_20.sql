
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

ALTER TABLE IF EXISTS public.tblattribute
    ADD PRIMARY KEY (fldattributeid);

--
-- TOC entry 217 (class 1259 OID 16403)
-- Name: tblperson; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblperson (

    fldpersonid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
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

ALTER TABLE IF EXISTS public.tblperson
    ADD PRIMARY KEY (fldpersonid);

--
-- TOC entry 219 (class 1259 OID 16413)
-- Name: tblcase; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcase (
    fldcaseid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
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

ALTER TABLE IF EXISTS public.tblcase
    ADD PRIMARY KEY (fldcaseid);

--
-- TOC entry 221 (class 1259 OID 16420)
-- Name: tblcaserow; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcaserow (
    fldcaserowid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fldcaseid bigint,
    fldactiondesc character varying(255),
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone,
    fldcreatedby bigint,
    fldupdatedby bigint
);


ALTER TABLE public.tblcaserow OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblcaserow
    ADD PRIMARY KEY (fldcaserowid);


--
-- TOC entry 223 (class 1259 OID 16424)
-- Name: tblcasestatus; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcasestatus (
    fldcasestatusid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fldcasestatusdesc character varying(50),
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblcasestatus OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblcasestatus
    ADD PRIMARY KEY (fldcasestatusid);


--
-- TOC entry 225 (class 1259 OID 16428)
-- Name: tblcasetype; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblcasetype (
    fldcasetypeid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fldcasetypedesc character varying(100) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblcasetype OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblcasetype
    ADD PRIMARY KEY (fldcasetypeid);


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

ALTER TABLE IF EXISTS public.tbldashboard
    ADD PRIMARY KEY (fldpersonid);


--
-- TOC entry 227 (class 1259 OID 16435)
-- Name: tbldomitype; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbldomitype (
    fkddomitypeid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fkddomitypedesc character varying(50) NOT NULL
);


ALTER TABLE public.tbldomitype OWNER TO postgres;

ALTER TABLE IF EXISTS public.tbldomitype
    ADD PRIMARY KEY (fkddomitypeid);


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

ALTER TABLE IF EXISTS public.tbleklogikidomi
    ADD PRIMARY KEY (flddomiid);

--
-- TOC entry 229 (class 1259 OID 16441)
-- Name: tblentity; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblentity (
    fldentityid bigint GENERATED BY DEFAULT AS IDENTITY  NOT NULL,
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

ALTER TABLE IF EXISTS public.tblentity
    ADD PRIMARY KEY (fldentityid);

--
-- TOC entry 231 (class 1259 OID 16447)
-- Name: tblentitytype; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblentitytype (
    fldentitytypeid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fldentitytypedesc character varying(255) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblentitytype OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblentitytype
    ADD PRIMARY KEY (fldentitytypeid);


--
-- TOC entry 233 (class 1259 OID 16451)
-- Name: tblimportedperson; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblimportedperson (
    fldimportedpersonid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
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

ALTER TABLE IF EXISTS public.tblimportedperson
    ADD PRIMARY KEY (fldimportedpersonid);

--
-- TOC entry 235 (class 1259 OID 16457)
-- Name: tblinvitation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblinvitation (
    fldinvitationid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    fldtitle character varying(255),
    fldlocation character varying(255),
    fldhost character varying(255),
    fldentityid bigint,
    fldhostpersonid bigint,
    fldgopersonid bigint,
    fldcomments text,
    fldreplied bigint DEFAULT 0,
    flddatetime timestamp without time zone,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone    
);


ALTER TABLE public.tblinvitation OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblinvitation
    ADD PRIMARY KEY (fldinvitationid);

--
-- TOC entry 264 (class 1259 OID 16670)
-- Name: tblinvitationreplies; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblinvitationreplies (
    fldreplyid bigint NOT NULL,
    fldreplydesc character varying(20)
);


ALTER TABLE public.tblinvitationreplies OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblinvitationreplies
    ADD PRIMARY KEY (fldreplyid);

--
-- TOC entry 237 (class 1259 OID 16465)
-- Name: tbltask; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbltask (
    fldtaskid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
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

ALTER TABLE IF EXISTS public.tbltask
    ADD PRIMARY KEY (fldtaskid);


CREATE TABLE public.tblpersonattribute (
    fldpersonattrid bigint GENERATED BY DEFAULT AS IDENTITY  NOT NULL,
    fldpersonid bigint NOT NULL,
    fldattributeid bigint NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblpersonattribute OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblpersonattribute
    ADD PRIMARY KEY (fldpersonattrid);

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


ALTER TABLE IF EXISTS public.tblpersonattributesaggregate
    ADD PRIMARY KEY (fldpersonid);

--
-- TOC entry 242 (class 1259 OID 16480)
-- Name: tblprofession; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblprofession (
    fldprofessionid bigint GENERATED BY DEFAULT AS IDENTITY  NOT NULL,
    fldprofessionname character varying(50) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblprofession OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblprofession
    ADD PRIMARY KEY (fldprofessionid);

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

ALTER TABLE IF EXISTS public.tblrefreshtoken
    ADD PRIMARY KEY (flduserid);

--
-- TOC entry 245 (class 1259 OID 16487)
-- Name: tblreport; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblreport (
    fldreportid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
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

ALTER TABLE IF EXISTS public.tblreport
    ADD PRIMARY KEY (fldreportid);

--
-- TOC entry 246 (class 1259 OID 16492)
-- Name: tblreport_fldcreatedby_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--


--
-- TOC entry 250 (class 1259 OID 16496)
-- Name: tblrole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblrole (
    fldroleid bigint GENERATED BY DEFAULT AS IDENTITY  NOT NULL,
    fldroledesc character varying(30) NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tblrole OWNER TO postgres;

ALTER TABLE IF EXISTS public.tblrole
    ADD PRIMARY KEY (fldroleid);

--
-- TOC entry 252 (class 1259 OID 16500)
-- Name: tbluser; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbluser (
    flduserid bigint GENERATED BY DEFAULT AS IDENTITY  NOT NULL,
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

ALTER TABLE IF EXISTS public.tbluser
    ADD PRIMARY KEY (flduserid);

-- TOC entry 254 (class 1259 OID 16506)
-- Name: tbluserrole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tbluserrole (
    flduserroleid bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    flduserid bigint NOT NULL,
    fldroleid bigint NOT NULL,
    fldcreatedby bigint,
    fldupdatedby bigint,
    fldcreatedat timestamp without time zone,
    fldupdatedat timestamp without time zone
);


ALTER TABLE public.tbluserrole OWNER TO postgres;

ALTER TABLE IF EXISTS public.tbluserrole
    ADD PRIMARY KEY (flduserroleid);

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