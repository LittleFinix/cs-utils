using System;

namespace Finix.CsUtils.MARC
{
    public enum RelatorType
    {
        /// [abr] A person
        Abridger,

        /// [act] A performer contributing to an expression of a work by acting as a cast member or player in a musical or dramatic presentation
        Actor,

        /// [adp] A person or organization who 1) reworks a musical composition
        Adapter,

        /// [rcp] A person
        Addressee,

        /// [anl] A person or organization that reviews
        Analyst,

        /// [anm] A person contributing to a moving image work or computer program by giving apparent movement to inanimate objects or drawings. For the creator of the drawings that are animated
        Animator,

        /// [ann] A person who makes manuscript annotations on an item
        Annotator,

        /// [apl] A person or organization who appeals a lower court's decision
        Appellant,

        /// [ape] A person or organization against whom an appeal is taken
        Appellee,

        /// [app] A person or organization responsible for the submission of an application or who is named as eligible for the results of the processing of the application (e.g.
        Applicant,

        /// [arc] A person
        Architect,

        /// [arr] A person
        Arranger,

        /// [acp] A person (e.g.
        ArtCopyist,

        /// [adi] A person contributing to a motion picture or television production by overseeing the artists and craftspeople who build the sets
        ArtDirector,

        /// [art] A person
        Artist,

        /// [ard] A person responsible for controlling the development of the artistic style of an entire production
        ArtisticDirector,

        /// [asg] A person or organization to whom a license for printing or publishing has been transferred
        Assignee,

        /// [asn] A person or organization associated with or found in an item or collection
        AssociatedName,

        /// [att] An author
        AttributedName,

        /// [auc] A person or organization in charge of the estimation and public auctioning of goods
        Auctioneer,

        /// [aut] A person
        Author,

        /// [aqt] A person or organization whose work is largely quoted or extracted in works to which he or she did not contribute directly. Such quotations are found particularly in exhibition catalogs
        AuthorInQuotationsOrTextAbstracts,

        /// [ colophon]  etc.
        AuthorOfAfterword,

        /// [aud] A person or organization responsible for the dialog or spoken commentary for a screenplay or sound recording
        AuthorOfDialog,

        /// [ etc.] aui
        AuthorOfIntroduction,

        /// [ato] A person whose manuscript signature appears on an item
        Autographer,

        /// [ant] A person or organization responsible for a resource upon which the resource represented by the bibliographic description is based. This may be appropriate for adaptations
        BibliographicAntecedent,

        /// [bnd] A person who binds an item
        Binder,

        /// [bdd] A person or organization responsible for the binding design of a book
        BindingDesigner,

        /// [blw] A person or organization responsible for writing a commendation or testimonial for a work
        BlurbWriter,

        /// [bkd] A person or organization involved in manufacturing a manifestation by being responsible for the entire graphic design of a book
        BookDesigner,

        /// [bkp] A person or organization responsible for the production of books and other print media UF Producer of book
        BookProducer,

        /// [bjd] A person or organization responsible for the design of flexible covers designed for or published with a book
        BookjacketDesigner,

        /// [bpd] A person or organization responsible for the design of a book owner's identification label that is most commonly pasted to the inside front cover of a book
        BookplateDesigner,

        /// [bsl] A person or organization who makes books and other bibliographic materials available for purchase. Interest in the materials is primarily lucrative
        Bookseller,

        /// [brl] A person
        BrailleEmbosser,

        /// [brd] A person
        Broadcaster,

        /// [cll] A person or organization who writes in an artistic hand
        Calligrapher,

        /// [ctg] A person
        Cartographer,

        /// [cas] A person
        Caster,

        /// [cns] A person or organization who examines bibliographic resources for the purpose of suppressing parts deemed objectionable on moral
        Censor,

        /// [chr] A person responsible for creating or contributing to a work of movement
        Choreographer,

        /// [cng] A person in charge of photographing a motion picture
        Cinematographer,

        /// [cli] A person or organization for whom another person or organization is acting
        Client,

        /// [cor] A curator who lists or inventories the items in an aggregate work such as a collection of items or works
        CollectionRegistrar,

        /// [col] A curator who brings together items from various sources that are then arranged
        Collector,

        /// [clt] A person
        Collotyper,

        /// [clr] A person or organization responsible for applying color to drawings
        Colorist,

        /// [cmm] A performer contributing to a work by providing interpretation
        Commentator,

        /// [cwt] A person or organization responsible for the commentary or explanatory notes about a text. For the writer of manuscript annotations in a printed book
        CommentatorForWrittenText,

        /// [com] A person
        Compiler,

        /// [cpl] A person or organization who applies to the courts for redress
        Complainant,

        /// [cpt] A complainant who takes an appeal from one court or jurisdiction to another to reverse the judgment
        ComplainantAppellant,

        /// [cpe] A complainant against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment
        ComplainantAppellee,

        /// [cmp] A person
        Composer,

        /// [cmt] A person or organization responsible for the creation of metal slug
        Compositor,

        /// [ccp] A person or organization responsible for the original idea on which a work is based
        Conceptor,

        /// [cnd] A performer contributing to a musical resource by leading a performing group (orchestra
        Conductor,

        /// [con] A person or organization responsible for documenting
        Conservator,

        /// [csl] A person or organization relevant to a resource
        Consultant,

        /// [csp] A person or organization relevant to a resource
        ConsultantToAProject,

        /// [cos] A person(s) or organization who opposes
        Contestant,

        /// [cot] A contestant who takes an appeal from one court of law or jurisdiction to another to reverse the judgment
        ContestantAppellant,

        /// [coe] A contestant against whom an appeal is taken from one court of law or jurisdiction to another to reverse the judgment
        ContestantAppellee,

        /// [cts] A person(s) or organization defending a claim
        Contestee,

        /// [ctt] A contestee who takes an appeal from one court or jurisdiction to another to reverse the judgment
        ContesteeAppellant,

        /// [cte] A contestee against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment
        ContesteeAppellee,

        /// [ctr] A person or organization relevant to a resource
        Contractor,

        /// [ctb] A person
        Contributor,

        /// [cpc] A person or organization listed as a copyright owner at the time of registration. Copyright can be granted or later transferred to another person or organization
        CopyrightClaimant,

        /// [cph] A person or organization to whom copy and legal rights have been granted or transferred for the intellectual content of a work. The copyright holder
        CopyrightHolder,

        /// [crr] A person or organization who is a corrector of manuscripts
        Corrector,

        /// [crp] A person or organization who was either the writer or recipient of a letter or other communication
        Correspondent,

        /// [cst] A person
        CostumeDesigner,

        /// [cou] A court governed by court rules
        CourtGoverned,

        /// [crt] A person
        CourtReporter,

        /// [cov] A person or organization responsible for the graphic design of a book cover
        CoverDesigner,

        /// [cre] A person or organization responsible for the intellectual or artistic content of a resource
        Creator,

        /// [cur] A person
        Curator,

        /// [dnc] A performer who dances in a musical
        Dancer,

        /// [dtc] A person or organization that submits data for inclusion in a database or other collection of data
        DataContributor,

        /// [dtm] A person or organization responsible for managing databases or other data sources
        DataManager,

        /// [dte] A person
        Dedicatee,

        /// [dto] A person who writes a dedication
        Dedicator,

        /// [dfd] A person or organization who is accused in a criminal proceeding or sued in a civil proceeding
        Defendant,

        /// [dft] A defendant who takes an appeal from one court or jurisdiction to another to reverse the judgment
        DefendantAppellant,

        /// [dfe] A defendant against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment
        DefendantAppellee,

        /// [dgg] A organization granting an academic degree UF Degree grantor
        DegreeGrantingInstitution,

        /// [dgs] A person overseeing a higher level academic degree
        DegreeSupervisor,

        /// [dln] A person or organization executing technical drawings from others' designs
        Delineator,

        /// [dpc] An entity depicted or portrayed in a work
        Depicted,

        /// [dpt] A current owner of an item who deposited the item into the custody of another person
        Depositor,

        /// [dsr] A person
        Designer,

        /// [drt] A person responsible for the general management and supervision of a filmed performance
        Director,

        /// [dis] A person who presents a thesis for a university or higher-level educational degree
        Dissertant,

        /// [dbp] A place from which a resource
        DistributionPlace,

        /// [dst] A person or organization that has exclusive or shared marketing rights for a resource
        Distributor,

        /// [dnr] A former owner of an item who donated that item to another owner
        Donor,

        /// [drm] A person
        Draftsman,

        /// [dub] A person or organization to which authorship has been dubiously or incorrectly ascribed
        DubiousAuthor,

        /// [edt] A person
        Editor,

        /// [edc] A person
        EditorOfCompilation,

        /// [edm] A person
        EditorOfMovingImageWork,

        /// [elg] A person responsible for setting up a lighting rig and focusing the lights for a production
        Electrician,

        /// [elt] A person or organization who creates a duplicate printing surface by pressure molding and electrodepositing of metal that is then backed up with lead for printing
        Electrotyper,

        /// [enj] A jurisdiction enacting a law
        EnactingJurisdiction,

        /// [eng] A person or organization that is responsible for technical planning and design
        Engineer,

        /// [egr] A person or organization who cuts letters
        Engraver,

        /// [etr] A person or organization who produces text or images for printing by subjecting metal
        Etcher,

        /// [evp] A place where an event such as a conference or a concert took place
        EventPlace,

        /// [exp] A person or organization in charge of the description and appraisal of the value of goods
        Expert,

        /// [fac] A person or organization that executed the facsimile UF Copier
        Facsimilist,

        /// [fld] A person or organization that manages or supervises the work done to collect raw data or do research in an actual setting or environment (typically applies to the natural and social sciences)
        FieldDirector,

        /// [fmd] A director responsible for the general management and supervision of a filmed performance
        FilmDirector,

        /// [fds] A person
        FilmDistributor,

        /// [flm] A person who
        FilmEditor,

        /// [fmp] A producer responsible for most of the business aspects of a film
        FilmProducer,

        /// [fmk] A person
        Filmmaker,

        /// [fpy] A person or organization who is identified as the only party or the party of the first party. In the case of transfer of rights
        FirstParty,

        /// [frg] A person or organization who makes or imitates something of value or importance
        Forger,

        /// [fmo] A person
        FormerOwner,

        /// [fnd] A person or organization that furnished financial support for the production of the work
        Funder,

        /// [gis] A person responsible for geographic information system (GIS) development and integration with global positioning system data UF Geospatial information specialist
        GeographicInformationSpecialist,

        /// [hnr] A person
        Honoree,

        /// [hst] A performer contributing to a resource by leading a program (often broadcast) that includes other guests
        Host,

        /// [his] An organization hosting the event
        HostInstitution,

        /// [ilu] A person providing decoration to a specific item using precious metals or color
        Illuminator,

        /// [ill] A person
        Illustrator,

        /// [ins] A person who has written a statement of dedication or gift
        Inscriber,

        /// [itr] A performer contributing to a resource by playing a musical instrument
        Instrumentalist,

        /// [ive] A person
        Interviewee,

        /// [ivr] A person
        Interviewer,

        /// [inv] A person
        Inventor,

        /// [isb] A person
        IssuingBody,

        /// [jud] A person who hears and decides on legal matters in court.
        Judge,

        /// [jug] A jurisdiction governed by a law
        JurisdictionGoverned,

        /// [lbr] An organization that provides scientific analyses of material samples
        Laboratory,

        /// [ldr] A person or organization that manages or supervises work done in a controlled setting or environment UF Lab director
        LaboratoryDirector,

        /// [lsa] An architect responsible for creating landscape works. This work involves coordinating the arrangement of existing and proposed land features and structures
        LandscapeArchitect,

        /// [led] A person or organization that takes primary responsibility for a particular activity or endeavor. May be combined with another relator term or code to show the greater importance this person or organization has regarding that particular role. If more than one relator is assigned to a heading
        Lead,

        /// [len] A person or organization permitting the temporary use of a book
        Lender,

        /// [lil] A person or organization who files a libel in an ecclesiastical or admiralty case
        Libelant,

        /// [lit] A libelant who takes an appeal from one ecclesiastical court or admiralty to another to reverse the judgment
        LibelantAppellant,

        /// [lie] A libelant against whom an appeal is taken from one ecclesiastical court or admiralty to another to reverse the judgment
        LibelantAppellee,

        /// [lel] A person or organization against whom a libel has been filed in an ecclesiastical court or admiralty
        Libelee,

        /// [let] A libelee who takes an appeal from one ecclesiastical court or admiralty to another to reverse the judgment
        LibeleeAppellant,

        /// [lee] A libelee against whom an appeal is taken from one ecclesiastical court or admiralty to another to reverse the judgment
        LibeleeAppellee,

        /// [lbt] An author of a libretto of an opera or other stage work
        Librettist,

        /// [lse] A person or organization who is an original recipient of the right to print or publish
        Licensee,

        /// [lso] A person or organization who is a signer of the license
        Licensor,

        /// [lgd] A person or organization who designs the lighting scheme for a theatrical presentation
        LightingDesigner,

        /// [ltg] A person or organization who prepares the stone or plate for lithographic printing
        Lithographer,

        /// [lyr] An author of the words of a non-dramatic musical work (e.g. the text of a song)
        Lyricist,

        /// [mfp] The place of manufacture (e.g.
        ManufacturePlace,

        /// [mfr] A person or organization responsible for printing
        Manufacturer,

        /// [mrb] The entity responsible for marbling paper
        Marbler,

        /// [mrk] A person or organization performing the coding of SGML
        MarkupEditor,

        /// [med] A person held to be a channel of communication between the earthly world and a world
        Medium,

        /// [mdc] A person or organization primarily responsible for compiling and maintaining the original description of a metadata set (e.g.
        MetadataContact,

        /// [mte] An engraver responsible for decorations
        MetalEngraver,

        /// [mtk] A person
        MinuteTaker,

        /// [mod] A performer contributing to a resource by leading a program (often broadcast) where topics are discussed
        Moderator,

        /// [mon] A person or organization that supervises compliance with the contract and is responsible for the report and controls its distribution. Sometimes referred to as the grantee
        Monitor,

        /// [mcp] A person who transcribes or copies musical notation
        MusicCopyist,

        /// [msd] A person who coordinates the activities of the composer
        MusicalDirector,

        /// [mus] A person or organization who performs music or contributes to the musical content of a work when it is not possible or desirable to identify the function more precisely
        Musician,

        /// [nrt] A performer contributing to a resource by reading or speaking in order to give an account of an act
        Narrator,

        /// [osp] A performer contributing to an expression of a work by appearing on screen in nonfiction moving image materials or introductions to fiction moving image materials to provide contextual or background information. Use when another term (e.g.
        OnscreenPresenter,

        /// [opn] A person or organization responsible for opposing a thesis or dissertation
        Opponent,

        /// [orm] A person
        Organizer,

        /// [org] A person or organization performing the work
        Originator,

        /// [oth] A role that has no equivalent in the MARC list.
        Other,

        /// [own] A person
        Owner,

        /// [pan] A performer contributing to a resource by participating in a program (often broadcast) where topics are discussed
        Panelist,

        /// [ppm] A person or organization responsible for the production of paper
        Papermaker,

        /// [pta] A person or organization that applied for a patent
        PatentApplicant,

        /// [pth] A person or organization that was granted the patent referred to by the item UF Patentee
        PatentHolder,

        /// [pat] A person or organization responsible for commissioning a work. Usually a patron uses his or her means or influence to support the work of artists
        Patron,

        /// [prf] A person contributing to a resource by performing music
        Performer,

        /// [pma] An organization (usually a government agency) that issues permits under which work is accomplished
        PermittingAgency,

        /// [pht] A person
        Photographer,

        /// [ptf] A person or organization who brings a suit in a civil proceeding
        Plaintiff,

        /// [ptt] A plaintiff who takes an appeal from one court or jurisdiction to another to reverse the judgment
        PlaintiffAppellant,

        /// [pte] A plaintiff against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment
        PlaintiffAppellee,

        /// [plt] A person
        Platemaker,

        /// [pra] A person who is the faculty moderator of an academic disputation
        Praeses,

        /// [pre] A person or organization mentioned in an “X presents” credit for moving image materials and who is associated with production
        Presenter,

        /// [prt] A person
        Printer,

        /// [pop] A person or organization who prints illustrations from plates. UF Plates
        PrinterOfPlates,

        /// [prm] A person or organization who makes a relief
        Printmaker,

        /// [prc] A person or organization primarily responsible for performing or initiating a process
        ProcessContact,

        /// [pro] A person
        Producer,

        /// [prn] An organization that is responsible for financial
        ProductionCompany,

        /// [prs] A person or organization responsible for designing the overall visual appearance of a moving image production
        ProductionDesigner,

        /// [pmn] A person responsible for all technical and business matters in a production
        ProductionManager,

        /// [prd] A person or organization associated with the production (props
        ProductionPersonnel,

        /// [prp] The place of production (e.g.
        ProductionPlace,

        /// [prg] A person
        Programmer,

        /// [pdr] A person or organization with primary responsibility for all essential aspects of a project
        ProjectDirector,

        /// [pfr] A person who corrects printed matter. For manuscripts
        Proofreader,

        /// [prv] A person or organization who produces
        Provider,

        /// [pup] The place where a resource is published
        PublicationPlace,

        /// [pbl] A person or organization responsible for publishing
        Publisher,

        /// [pbd] A person or organization who presides over the elaboration of a collective work to ensure its coherence or continuity. This includes editors-in-chief
        PublishingDirector,

        /// [ppt] A performer contributing to a resource by manipulating
        Puppeteer,

        /// [rdd] A director responsible for the general management and supervision of a radio program
        RadioDirector,

        /// [rpc] A producer responsible for most of the business aspects of a radio program
        RadioProducer,

        /// [rce] A person contributing to a resource by supervising the technical aspects of a sound or video recording session
        RecordingEngineer,

        /// [rcd] A person or organization who uses a recording device to capture sounds and/or video during a recording session
        Recordist,

        /// [red] A person or organization who writes or develops the framework for an item without being intellectually responsible for its content
        Redaktor,

        /// [ren] A person or organization who prepares drawings of architectural designs (i.e.
        Renderer,

        /// [rpt] A person or organization who writes or presents reports of news or current events on air or in print
        Reporter,

        /// [rps] An organization that hosts data or material culture objects and provides services to promote long term
        Repository,

        /// [rth] A person who directed or managed a research project
        ResearchTeamHead,

        /// [rtm] A person who participated in a research project but whose role did not involve direction or management of it
        ResearchTeamMember,

        /// [res] A person or organization responsible for performing research UF Performer of research
        Researcher,

        /// [rsp] A person or organization who makes an answer to the courts pursuant to an application for redress (usually in an equity proceeding) or a candidate for a degree who defends or opposes a thesis provided by the praeses in an academic disputation
        Respondent,

        /// [rst] A respondent who takes an appeal from one court or jurisdiction to another to reverse the judgment
        RespondentAppellant,

        /// [rse] A respondent against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment
        RespondentAppellee,

        /// [rpy] A person or organization legally responsible for the content of the published material
        ResponsibleParty,

        /// [rsg] A person or organization
        Restager,

        /// [rsr] A person
        Restorationist,

        /// [rev] A person or organization responsible for the review of a book
        Reviewer,

        /// [rbr] A person or organization responsible for parts of a work
        Rubricator,

        /// [sce] A person or organization who is the author of a motion picture screenplay
        Scenarist,

        /// [sad] A person or organization who brings scientific
        ScientificAdvisor,

        /// [aus] An author of a screenplay
        Screenwriter,

        /// [scr] A person who is an amanuensis and for a writer of manuscripts proper. For a person who makes pen-facsimiles
        Scribe,

        /// [scl] An artist responsible for creating a three-dimensional work by modeling
        Sculptor,

        /// [spy] A person or organization who is identified as the party of the second part. In the case of transfer of right
        SecondParty,

        /// [sec] A person or organization who is a recorder
        Secretary,

        /// [sll] A former owner of an item who sold that item to another owner
        Seller,

        /// [std] A person who translates the rough sketches of the art director into actual architectural structures for a theatrical presentation
        SetDesigner,

        /// [stg] An entity in which the activity or plot of a work takes place
        Setting,

        /// [sgn] A person whose signature appears without a presentation or other statement indicative of provenance. When there is a presentation statement
        Signer,

        /// [sng] A performer contributing to a resource by using his/her/their voice
        Singer,

        /// [sds] A person who produces and reproduces the sound score (both live and recorded)
        SoundDesigner,

        /// [spk] A performer contributing to a resource by speaking words
        Speaker,

        /// [spn] A person
        Sponsor,

        /// [sgd] A person or organization contributing to a stage resource through the overall management and supervision of a performance
        StageDirector,

        /// [stm] A person who is in charge of everything that occurs on a performance stage
        StageManager,

        /// [stn] An organization responsible for the development or enforcement of a standard
        StandardsBody,

        /// [str] A person or organization who creates a new plate for printing by molding or copying another printing surface
        Stereotyper,

        /// [stl] A performer contributing to a resource by relaying a creator's original story with dramatic or theatrical interpretation
        Storyteller,

        /// [sht] A person or organization that supports (by allocating facilities
        SupportingHost,

        /// [srv] A person
        Surveyor,

        /// [tch] A performer contributing to a resource by giving instruction or providing a demonstration UF Instructor
        Teacher,

        /// [tcd] A person who is ultimately in charge of scenery
        TechnicalDirector,

        /// [tld] A director responsible for the general management and supervision of a television program
        TelevisionDirector,

        /// [tlp] A producer responsible for most of the business aspects of a television program
        TelevisionProducer,

        /// [ths] A person under whose supervision a degree candidate develops and presents a thesis
        ThesisAdvisor,

        /// [trc] A person
        Transcriber,

        /// [trl] A person or organization who renders a text from one language into another
        Translator,

        /// [tyd] A person or organization who designs the type face used in a particular item UF Designer of type
        TypeDesigner,

        /// [tyg] A person or organization primarily responsible for choice and arrangement of type used in an item. If the typographer is also responsible for other aspects of the graphic design of a book (e.g.
        Typographer,

        /// [uvp] A place where a university that is associated with a resource is located
        UniversityPlace,

        /// [vdg] A person in charge of a video production
        Videographer,

        /// [vac] An actor contributing to a resource by providing the voice for characters in radio and audio productions and for animated characters in moving image works
        VoiceActor,

        /// [wit] Use for a person who verifies the truthfulness of an event or action. UF Deponent UF Eyewitness UF Observer UF Onlooker UF Testifier
        Witness,

        /// [wde] A person or organization who makes prints by cutting the image in relief on the end-grain of a wood block
        WoodEngraver,

        /// [wdc] A person or organization who makes prints by cutting the image in relief on the plank side of a wood block
        Woodcutter,

        /// [wam] A person or organization who writes significant material which accompanies a sound recording or other audiovisual material
        WriterOfAccompanyingMaterial,

        /// [wac] A person
        WriterOfAddedCommentary,

        /// [wal] A writer of words added to an expression of a musical work. For lyric writing in collaboration with a composer to form an original work
        WriterOfAddedLyrics,

        /// [wat] A person
        WriterOfAddedText,

        /// [win] A person
        WriterOfIntroduction,

        /// [wpr] A person
        WriterOfPreface,

        /// [wst] A person
        WriterOfSupplementaryTextualContent,
    }

    public static class Relator
    {
        public static string ToString(RelatorType rel)
        {
            switch (rel)
            {
                case RelatorType.Expert:
                    return "exp";

                case RelatorType.Abridger:
                    return "abr";

                case RelatorType.Actor:
                    return "act";

                case RelatorType.Adapter:
                    return "adp";

                case RelatorType.Addressee:
                    return "rcp";

                case RelatorType.Analyst:
                    return "anl";

                case RelatorType.Animator:
                    return "anm";

                case RelatorType.Annotator:
                    return "ann";

                case RelatorType.Appellant:
                    return "apl";

                case RelatorType.Appellee:
                    return "ape";

                case RelatorType.Applicant:
                    return "app";

                case RelatorType.Architect:
                    return "arc";

                case RelatorType.Arranger:
                    return "arr";

                case RelatorType.ArtCopyist:
                    return "acp";

                case RelatorType.ArtDirector:
                    return "adi";

                case RelatorType.Artist:
                    return "art";

                case RelatorType.ArtisticDirector:
                    return "ard";

                case RelatorType.Assignee:
                    return "asg";

                case RelatorType.AssociatedName:
                    return "asn";

                case RelatorType.AttributedName:
                    return "att";

                case RelatorType.Auctioneer:
                    return "auc";

                case RelatorType.Author:
                    return "aut";

                case RelatorType.AuthorInQuotationsOrTextAbstracts:
                    return "aqt";

                case RelatorType.AuthorOfAfterword:
                    return " colophon";

                case RelatorType.AuthorOfDialog:
                    return "aud";

                case RelatorType.AuthorOfIntroduction:
                    return " etc.";

                case RelatorType.Autographer:
                    return "ato";

                case RelatorType.BibliographicAntecedent:
                    return "ant";

                case RelatorType.Binder:
                    return "bnd";

                case RelatorType.BindingDesigner:
                    return "bdd";

                case RelatorType.BlurbWriter:
                    return "blw";

                case RelatorType.BookDesigner:
                    return "bkd";

                case RelatorType.BookProducer:
                    return "bkp";

                case RelatorType.BookjacketDesigner:
                    return "bjd";

                case RelatorType.BookplateDesigner:
                    return "bpd";

                case RelatorType.Bookseller:
                    return "bsl";

                case RelatorType.BrailleEmbosser:
                    return "brl";

                case RelatorType.Broadcaster:
                    return "brd";

                case RelatorType.Calligrapher:
                    return "cll";

                case RelatorType.Cartographer:
                    return "ctg";

                case RelatorType.Caster:
                    return "cas";

                case RelatorType.Censor:
                    return "cns";

                case RelatorType.Choreographer:
                    return "chr";

                case RelatorType.Cinematographer:
                    return "cng";

                case RelatorType.Client:
                    return "cli";

                case RelatorType.CollectionRegistrar:
                    return "cor";

                case RelatorType.Collector:
                    return "col";

                case RelatorType.Collotyper:
                    return "clt";

                case RelatorType.Colorist:
                    return "clr";

                case RelatorType.Commentator:
                    return "cmm";

                case RelatorType.CommentatorForWrittenText:
                    return "cwt";

                case RelatorType.Compiler:
                    return "com";

                case RelatorType.Complainant:
                    return "cpl";

                case RelatorType.ComplainantAppellant:
                    return "cpt";

                case RelatorType.ComplainantAppellee:
                    return "cpe";

                case RelatorType.Composer:
                    return "cmp";

                case RelatorType.Compositor:
                    return "cmt";

                case RelatorType.Conceptor:
                    return "ccp";

                case RelatorType.Conductor:
                    return "cnd";

                case RelatorType.Conservator:
                    return "con";

                case RelatorType.Consultant:
                    return "csl";

                case RelatorType.ConsultantToAProject:
                    return "csp";

                case RelatorType.Contestant:
                    return "cos";

                case RelatorType.ContestantAppellant:
                    return "cot";

                case RelatorType.ContestantAppellee:
                    return "coe";

                case RelatorType.Contestee:
                    return "cts";

                case RelatorType.ContesteeAppellant:
                    return "ctt";

                case RelatorType.ContesteeAppellee:
                    return "cte";

                case RelatorType.Contractor:
                    return "ctr";

                case RelatorType.Contributor:
                    return "ctb";

                case RelatorType.CopyrightClaimant:
                    return "cpc";

                case RelatorType.CopyrightHolder:
                    return "cph";

                case RelatorType.Corrector:
                    return "crr";

                case RelatorType.Correspondent:
                    return "crp";

                case RelatorType.CostumeDesigner:
                    return "cst";

                case RelatorType.CourtGoverned:
                    return "cou";

                case RelatorType.CourtReporter:
                    return "crt";

                case RelatorType.CoverDesigner:
                    return "cov";

                case RelatorType.Creator:
                    return "cre";

                case RelatorType.Curator:
                    return "cur";

                case RelatorType.Dancer:
                    return "dnc";

                case RelatorType.DataContributor:
                    return "dtc";

                case RelatorType.DataManager:
                    return "dtm";

                case RelatorType.Dedicatee:
                    return "dte";

                case RelatorType.Dedicator:
                    return "dto";

                case RelatorType.Defendant:
                    return "dfd";

                case RelatorType.DefendantAppellant:
                    return "dft";

                case RelatorType.DefendantAppellee:
                    return "dfe";

                case RelatorType.DegreeGrantingInstitution:
                    return "dgg";

                case RelatorType.DegreeSupervisor:
                    return "dgs";

                case RelatorType.Delineator:
                    return "dln";

                case RelatorType.Depicted:
                    return "dpc";

                case RelatorType.Depositor:
                    return "dpt";

                case RelatorType.Designer:
                    return "dsr";

                case RelatorType.Director:
                    return "drt";

                case RelatorType.Dissertant:
                    return "dis";

                case RelatorType.DistributionPlace:
                    return "dbp";

                case RelatorType.Distributor:
                    return "dst";

                case RelatorType.Donor:
                    return "dnr";

                case RelatorType.Draftsman:
                    return "drm";

                case RelatorType.DubiousAuthor:
                    return "dub";

                case RelatorType.Editor:
                    return "edt";

                case RelatorType.EditorOfCompilation:
                    return "edc";

                case RelatorType.EditorOfMovingImageWork:
                    return "edm";

                case RelatorType.Electrician:
                    return "elg";

                case RelatorType.Electrotyper:
                    return "elt";

                case RelatorType.EnactingJurisdiction:
                    return "enj";

                case RelatorType.Engineer:
                    return "eng";

                case RelatorType.Engraver:
                    return "egr";

                case RelatorType.Etcher:
                    return "etr";

                case RelatorType.EventPlace:
                    return "evp";

                case RelatorType.Facsimilist:
                    return "fac";

                case RelatorType.FieldDirector:
                    return "fld";

                case RelatorType.FilmDirector:
                    return "fmd";

                case RelatorType.FilmDistributor:
                    return "fds";

                case RelatorType.FilmEditor:
                    return "flm";

                case RelatorType.FilmProducer:
                    return "fmp";

                case RelatorType.Filmmaker:
                    return "fmk";

                case RelatorType.FirstParty:
                    return "fpy";

                case RelatorType.Forger:
                    return "frg";

                case RelatorType.FormerOwner:
                    return "fmo";

                case RelatorType.Funder:
                    return "fnd";

                case RelatorType.GeographicInformationSpecialist:
                    return "gis";

                case RelatorType.Honoree:
                    return "hnr";

                case RelatorType.Host:
                    return "hst";

                case RelatorType.HostInstitution:
                    return "his";

                case RelatorType.Illuminator:
                    return "ilu";

                case RelatorType.Illustrator:
                    return "ill";

                case RelatorType.Inscriber:
                    return "ins";

                case RelatorType.Instrumentalist:
                    return "itr";

                case RelatorType.Interviewee:
                    return "ive";

                case RelatorType.Interviewer:
                    return "ivr";

                case RelatorType.Inventor:
                    return "inv";

                case RelatorType.IssuingBody:
                    return "isb";

                case RelatorType.Judge:
                    return "jud";

                case RelatorType.JurisdictionGoverned:
                    return "jug";

                case RelatorType.Laboratory:
                    return "lbr";

                case RelatorType.LaboratoryDirector:
                    return "ldr";

                case RelatorType.LandscapeArchitect:
                    return "lsa";

                case RelatorType.Lead:
                    return "led";

                case RelatorType.Lender:
                    return "len";

                case RelatorType.Libelant:
                    return "lil";

                case RelatorType.LibelantAppellant:
                    return "lit";

                case RelatorType.LibelantAppellee:
                    return "lie";

                case RelatorType.Libelee:
                    return "lel";

                case RelatorType.LibeleeAppellant:
                    return "let";

                case RelatorType.LibeleeAppellee:
                    return "lee";

                case RelatorType.Librettist:
                    return "lbt";

                case RelatorType.Licensee:
                    return "lse";

                case RelatorType.Licensor:
                    return "lso";

                case RelatorType.LightingDesigner:
                    return "lgd";

                case RelatorType.Lithographer:
                    return "ltg";

                case RelatorType.Lyricist:
                    return "lyr";

                case RelatorType.ManufacturePlace:
                    return "mfp";

                case RelatorType.Manufacturer:
                    return "mfr";

                case RelatorType.Marbler:
                    return "mrb";

                case RelatorType.MarkupEditor:
                    return "mrk";

                case RelatorType.Medium:
                    return "med";

                case RelatorType.MetadataContact:
                    return "mdc";

                case RelatorType.MetalEngraver:
                    return "mte";

                case RelatorType.MinuteTaker:
                    return "mtk";

                case RelatorType.Moderator:
                    return "mod";

                case RelatorType.Monitor:
                    return "mon";

                case RelatorType.MusicCopyist:
                    return "mcp";

                case RelatorType.MusicalDirector:
                    return "msd";

                case RelatorType.Musician:
                    return "mus";

                case RelatorType.Narrator:
                    return "nrt";

                case RelatorType.OnscreenPresenter:
                    return "osp";

                case RelatorType.Opponent:
                    return "opn";

                case RelatorType.Organizer:
                    return "orm";

                case RelatorType.Originator:
                    return "org";

                case RelatorType.Other:
                    return "oth";

                case RelatorType.Owner:
                    return "own";

                case RelatorType.Panelist:
                    return "pan";

                case RelatorType.Papermaker:
                    return "ppm";

                case RelatorType.PatentApplicant:
                    return "pta";

                case RelatorType.PatentHolder:
                    return "pth";

                case RelatorType.Patron:
                    return "pat";

                case RelatorType.Performer:
                    return "prf";

                case RelatorType.PermittingAgency:
                    return "pma";

                case RelatorType.Photographer:
                    return "pht";

                case RelatorType.Plaintiff:
                    return "ptf";

                case RelatorType.PlaintiffAppellant:
                    return "ptt";

                case RelatorType.PlaintiffAppellee:
                    return "pte";

                case RelatorType.Platemaker:
                    return "plt";

                case RelatorType.Praeses:
                    return "pra";

                case RelatorType.Presenter:
                    return "pre";

                case RelatorType.Printer:
                    return "prt";

                case RelatorType.PrinterOfPlates:
                    return "pop";

                case RelatorType.Printmaker:
                    return "prm";

                case RelatorType.ProcessContact:
                    return "prc";

                case RelatorType.Producer:
                    return "pro";

                case RelatorType.ProductionCompany:
                    return "prn";

                case RelatorType.ProductionDesigner:
                    return "prs";

                case RelatorType.ProductionManager:
                    return "pmn";

                case RelatorType.ProductionPersonnel:
                    return "prd";

                case RelatorType.ProductionPlace:
                    return "prp";

                case RelatorType.Programmer:
                    return "prg";

                case RelatorType.ProjectDirector:
                    return "pdr";

                case RelatorType.Proofreader:
                    return "pfr";

                case RelatorType.Provider:
                    return "prv";

                case RelatorType.PublicationPlace:
                    return "pup";

                case RelatorType.Publisher:
                    return "pbl";

                case RelatorType.PublishingDirector:
                    return "pbd";

                case RelatorType.Puppeteer:
                    return "ppt";

                case RelatorType.RadioDirector:
                    return "rdd";

                case RelatorType.RadioProducer:
                    return "rpc";

                case RelatorType.RecordingEngineer:
                    return "rce";

                case RelatorType.Recordist:
                    return "rcd";

                case RelatorType.Redaktor:
                    return "red";

                case RelatorType.Renderer:
                    return "ren";

                case RelatorType.Reporter:
                    return "rpt";

                case RelatorType.Repository:
                    return "rps";

                case RelatorType.ResearchTeamHead:
                    return "rth";

                case RelatorType.ResearchTeamMember:
                    return "rtm";

                case RelatorType.Researcher:
                    return "res";

                case RelatorType.Respondent:
                    return "rsp";

                case RelatorType.RespondentAppellant:
                    return "rst";

                case RelatorType.RespondentAppellee:
                    return "rse";

                case RelatorType.ResponsibleParty:
                    return "rpy";

                case RelatorType.Restager:
                    return "rsg";

                case RelatorType.Restorationist:
                    return "rsr";

                case RelatorType.Reviewer:
                    return "rev";

                case RelatorType.Rubricator:
                    return "rbr";

                case RelatorType.Scenarist:
                    return "sce";

                case RelatorType.ScientificAdvisor:
                    return "sad";

                case RelatorType.Screenwriter:
                    return "aus";

                case RelatorType.Scribe:
                    return "scr";

                case RelatorType.Sculptor:
                    return "scl";

                case RelatorType.SecondParty:
                    return "spy";

                case RelatorType.Secretary:
                    return "sec";

                case RelatorType.Seller:
                    return "sll";

                case RelatorType.SetDesigner:
                    return "std";

                case RelatorType.Setting:
                    return "stg";

                case RelatorType.Signer:
                    return "sgn";

                case RelatorType.Singer:
                    return "sng";

                case RelatorType.SoundDesigner:
                    return "sds";

                case RelatorType.Speaker:
                    return "spk";

                case RelatorType.Sponsor:
                    return "spn";

                case RelatorType.StageDirector:
                    return "sgd";

                case RelatorType.StageManager:
                    return "stm";

                case RelatorType.StandardsBody:
                    return "stn";

                case RelatorType.Stereotyper:
                    return "str";

                case RelatorType.Storyteller:
                    return "stl";

                case RelatorType.SupportingHost:
                    return "sht";

                case RelatorType.Surveyor:
                    return "srv";

                case RelatorType.Teacher:
                    return "tch";

                case RelatorType.TechnicalDirector:
                    return "tcd";

                case RelatorType.TelevisionDirector:
                    return "tld";

                case RelatorType.TelevisionProducer:
                    return "tlp";

                case RelatorType.ThesisAdvisor:
                    return "ths";

                case RelatorType.Transcriber:
                    return "trc";

                case RelatorType.Translator:
                    return "trl";

                case RelatorType.TypeDesigner:
                    return "tyd";

                case RelatorType.Typographer:
                    return "tyg";

                case RelatorType.UniversityPlace:
                    return "uvp";

                case RelatorType.Videographer:
                    return "vdg";

                case RelatorType.VoiceActor:
                    return "vac";

                case RelatorType.Witness:
                    return "wit";

                case RelatorType.WoodEngraver:
                    return "wde";

                case RelatorType.Woodcutter:
                    return "wdc";

                case RelatorType.WriterOfAccompanyingMaterial:
                    return "wam";

                case RelatorType.WriterOfAddedCommentary:
                    return "wac";

                case RelatorType.WriterOfAddedLyrics:
                    return "wal";

                case RelatorType.WriterOfAddedText:
                    return "wat";

                case RelatorType.WriterOfIntroduction:
                    return "win";

                case RelatorType.WriterOfPreface:
                    return "wpr";

                case RelatorType.WriterOfSupplementaryTextualContent:
                    return "wst";

                default:
                    throw new ArgumentException($"Unknown RelatorType: {rel}", nameof(rel));
            }
        }

        public static RelatorType Parse(string rel)
        {
            switch (rel)
            {
                case "exp":
                    return RelatorType.Expert;

                case "abr":
                    return RelatorType.Abridger;

                case "act":
                    return RelatorType.Actor;

                case "adp":
                    return RelatorType.Adapter;

                case "rcp":
                    return RelatorType.Addressee;

                case "anl":
                    return RelatorType.Analyst;

                case "anm":
                    return RelatorType.Animator;

                case "ann":
                    return RelatorType.Annotator;

                case "apl":
                    return RelatorType.Appellant;

                case "ape":
                    return RelatorType.Appellee;

                case "app":
                    return RelatorType.Applicant;

                case "arc":
                    return RelatorType.Architect;

                case "arr":
                    return RelatorType.Arranger;

                case "acp":
                    return RelatorType.ArtCopyist;

                case "adi":
                    return RelatorType.ArtDirector;

                case "art":
                    return RelatorType.Artist;

                case "ard":
                    return RelatorType.ArtisticDirector;

                case "asg":
                    return RelatorType.Assignee;

                case "asn":
                    return RelatorType.AssociatedName;

                case "att":
                    return RelatorType.AttributedName;

                case "auc":
                    return RelatorType.Auctioneer;

                case "aut":
                    return RelatorType.Author;

                case "aqt":
                    return RelatorType.AuthorInQuotationsOrTextAbstracts;

                case " colophon":
                    return RelatorType.AuthorOfAfterword;

                case "aud":
                    return RelatorType.AuthorOfDialog;

                case " etc.":
                    return RelatorType.AuthorOfIntroduction;

                case "ato":
                    return RelatorType.Autographer;

                case "ant":
                    return RelatorType.BibliographicAntecedent;

                case "bnd":
                    return RelatorType.Binder;

                case "bdd":
                    return RelatorType.BindingDesigner;

                case "blw":
                    return RelatorType.BlurbWriter;

                case "bkd":
                    return RelatorType.BookDesigner;

                case "bkp":
                    return RelatorType.BookProducer;

                case "bjd":
                    return RelatorType.BookjacketDesigner;

                case "bpd":
                    return RelatorType.BookplateDesigner;

                case "bsl":
                    return RelatorType.Bookseller;

                case "brl":
                    return RelatorType.BrailleEmbosser;

                case "brd":
                    return RelatorType.Broadcaster;

                case "cll":
                    return RelatorType.Calligrapher;

                case "ctg":
                    return RelatorType.Cartographer;

                case "cas":
                    return RelatorType.Caster;

                case "cns":
                    return RelatorType.Censor;

                case "chr":
                    return RelatorType.Choreographer;

                case "cng":
                    return RelatorType.Cinematographer;

                case "cli":
                    return RelatorType.Client;

                case "cor":
                    return RelatorType.CollectionRegistrar;

                case "col":
                    return RelatorType.Collector;

                case "clt":
                    return RelatorType.Collotyper;

                case "clr":
                    return RelatorType.Colorist;

                case "cmm":
                    return RelatorType.Commentator;

                case "cwt":
                    return RelatorType.CommentatorForWrittenText;

                case "com":
                    return RelatorType.Compiler;

                case "cpl":
                    return RelatorType.Complainant;

                case "cpt":
                    return RelatorType.ComplainantAppellant;

                case "cpe":
                    return RelatorType.ComplainantAppellee;

                case "cmp":
                    return RelatorType.Composer;

                case "cmt":
                    return RelatorType.Compositor;

                case "ccp":
                    return RelatorType.Conceptor;

                case "cnd":
                    return RelatorType.Conductor;

                case "con":
                    return RelatorType.Conservator;

                case "csl":
                    return RelatorType.Consultant;

                case "csp":
                    return RelatorType.ConsultantToAProject;

                case "cos":
                    return RelatorType.Contestant;

                case "cot":
                    return RelatorType.ContestantAppellant;

                case "coe":
                    return RelatorType.ContestantAppellee;

                case "cts":
                    return RelatorType.Contestee;

                case "ctt":
                    return RelatorType.ContesteeAppellant;

                case "cte":
                    return RelatorType.ContesteeAppellee;

                case "ctr":
                    return RelatorType.Contractor;

                case "ctb":
                    return RelatorType.Contributor;

                case "cpc":
                    return RelatorType.CopyrightClaimant;

                case "cph":
                    return RelatorType.CopyrightHolder;

                case "crr":
                    return RelatorType.Corrector;

                case "crp":
                    return RelatorType.Correspondent;

                case "cst":
                    return RelatorType.CostumeDesigner;

                case "cou":
                    return RelatorType.CourtGoverned;

                case "crt":
                    return RelatorType.CourtReporter;

                case "cov":
                    return RelatorType.CoverDesigner;

                case "cre":
                    return RelatorType.Creator;

                case "cur":
                    return RelatorType.Curator;

                case "dnc":
                    return RelatorType.Dancer;

                case "dtc":
                    return RelatorType.DataContributor;

                case "dtm":
                    return RelatorType.DataManager;

                case "dte":
                    return RelatorType.Dedicatee;

                case "dto":
                    return RelatorType.Dedicator;

                case "dfd":
                    return RelatorType.Defendant;

                case "dft":
                    return RelatorType.DefendantAppellant;

                case "dfe":
                    return RelatorType.DefendantAppellee;

                case "dgg":
                    return RelatorType.DegreeGrantingInstitution;

                case "dgs":
                    return RelatorType.DegreeSupervisor;

                case "dln":
                    return RelatorType.Delineator;

                case "dpc":
                    return RelatorType.Depicted;

                case "dpt":
                    return RelatorType.Depositor;

                case "dsr":
                    return RelatorType.Designer;

                case "drt":
                    return RelatorType.Director;

                case "dis":
                    return RelatorType.Dissertant;

                case "dbp":
                    return RelatorType.DistributionPlace;

                case "dst":
                    return RelatorType.Distributor;

                case "dnr":
                    return RelatorType.Donor;

                case "drm":
                    return RelatorType.Draftsman;

                case "dub":
                    return RelatorType.DubiousAuthor;

                case "edt":
                    return RelatorType.Editor;

                case "edc":
                    return RelatorType.EditorOfCompilation;

                case "edm":
                    return RelatorType.EditorOfMovingImageWork;

                case "elg":
                    return RelatorType.Electrician;

                case "elt":
                    return RelatorType.Electrotyper;

                case "enj":
                    return RelatorType.EnactingJurisdiction;

                case "eng":
                    return RelatorType.Engineer;

                case "egr":
                    return RelatorType.Engraver;

                case "etr":
                    return RelatorType.Etcher;

                case "evp":
                    return RelatorType.EventPlace;

                case "fac":
                    return RelatorType.Facsimilist;

                case "fld":
                    return RelatorType.FieldDirector;

                case "fmd":
                    return RelatorType.FilmDirector;

                case "fds":
                    return RelatorType.FilmDistributor;

                case "flm":
                    return RelatorType.FilmEditor;

                case "fmp":
                    return RelatorType.FilmProducer;

                case "fmk":
                    return RelatorType.Filmmaker;

                case "fpy":
                    return RelatorType.FirstParty;

                case "frg":
                    return RelatorType.Forger;

                case "fmo":
                    return RelatorType.FormerOwner;

                case "fnd":
                    return RelatorType.Funder;

                case "gis":
                    return RelatorType.GeographicInformationSpecialist;

                case "hnr":
                    return RelatorType.Honoree;

                case "hst":
                    return RelatorType.Host;

                case "his":
                    return RelatorType.HostInstitution;

                case "ilu":
                    return RelatorType.Illuminator;

                case "ill":
                    return RelatorType.Illustrator;

                case "ins":
                    return RelatorType.Inscriber;

                case "itr":
                    return RelatorType.Instrumentalist;

                case "ive":
                    return RelatorType.Interviewee;

                case "ivr":
                    return RelatorType.Interviewer;

                case "inv":
                    return RelatorType.Inventor;

                case "isb":
                    return RelatorType.IssuingBody;

                case "jud":
                    return RelatorType.Judge;

                case "jug":
                    return RelatorType.JurisdictionGoverned;

                case "lbr":
                    return RelatorType.Laboratory;

                case "ldr":
                    return RelatorType.LaboratoryDirector;

                case "lsa":
                    return RelatorType.LandscapeArchitect;

                case "led":
                    return RelatorType.Lead;

                case "len":
                    return RelatorType.Lender;

                case "lil":
                    return RelatorType.Libelant;

                case "lit":
                    return RelatorType.LibelantAppellant;

                case "lie":
                    return RelatorType.LibelantAppellee;

                case "lel":
                    return RelatorType.Libelee;

                case "let":
                    return RelatorType.LibeleeAppellant;

                case "lee":
                    return RelatorType.LibeleeAppellee;

                case "lbt":
                    return RelatorType.Librettist;

                case "lse":
                    return RelatorType.Licensee;

                case "lso":
                    return RelatorType.Licensor;

                case "lgd":
                    return RelatorType.LightingDesigner;

                case "ltg":
                    return RelatorType.Lithographer;

                case "lyr":
                    return RelatorType.Lyricist;

                case "mfp":
                    return RelatorType.ManufacturePlace;

                case "mfr":
                    return RelatorType.Manufacturer;

                case "mrb":
                    return RelatorType.Marbler;

                case "mrk":
                    return RelatorType.MarkupEditor;

                case "med":
                    return RelatorType.Medium;

                case "mdc":
                    return RelatorType.MetadataContact;

                case "mte":
                    return RelatorType.MetalEngraver;

                case "mtk":
                    return RelatorType.MinuteTaker;

                case "mod":
                    return RelatorType.Moderator;

                case "mon":
                    return RelatorType.Monitor;

                case "mcp":
                    return RelatorType.MusicCopyist;

                case "msd":
                    return RelatorType.MusicalDirector;

                case "mus":
                    return RelatorType.Musician;

                case "nrt":
                    return RelatorType.Narrator;

                case "osp":
                    return RelatorType.OnscreenPresenter;

                case "opn":
                    return RelatorType.Opponent;

                case "orm":
                    return RelatorType.Organizer;

                case "org":
                    return RelatorType.Originator;

                case "oth":
                    return RelatorType.Other;

                case "own":
                    return RelatorType.Owner;

                case "pan":
                    return RelatorType.Panelist;

                case "ppm":
                    return RelatorType.Papermaker;

                case "pta":
                    return RelatorType.PatentApplicant;

                case "pth":
                    return RelatorType.PatentHolder;

                case "pat":
                    return RelatorType.Patron;

                case "prf":
                    return RelatorType.Performer;

                case "pma":
                    return RelatorType.PermittingAgency;

                case "pht":
                    return RelatorType.Photographer;

                case "ptf":
                    return RelatorType.Plaintiff;

                case "ptt":
                    return RelatorType.PlaintiffAppellant;

                case "pte":
                    return RelatorType.PlaintiffAppellee;

                case "plt":
                    return RelatorType.Platemaker;

                case "pra":
                    return RelatorType.Praeses;

                case "pre":
                    return RelatorType.Presenter;

                case "prt":
                    return RelatorType.Printer;

                case "pop":
                    return RelatorType.PrinterOfPlates;

                case "prm":
                    return RelatorType.Printmaker;

                case "prc":
                    return RelatorType.ProcessContact;

                case "pro":
                    return RelatorType.Producer;

                case "prn":
                    return RelatorType.ProductionCompany;

                case "prs":
                    return RelatorType.ProductionDesigner;

                case "pmn":
                    return RelatorType.ProductionManager;

                case "prd":
                    return RelatorType.ProductionPersonnel;

                case "prp":
                    return RelatorType.ProductionPlace;

                case "prg":
                    return RelatorType.Programmer;

                case "pdr":
                    return RelatorType.ProjectDirector;

                case "pfr":
                    return RelatorType.Proofreader;

                case "prv":
                    return RelatorType.Provider;

                case "pup":
                    return RelatorType.PublicationPlace;

                case "pbl":
                    return RelatorType.Publisher;

                case "pbd":
                    return RelatorType.PublishingDirector;

                case "ppt":
                    return RelatorType.Puppeteer;

                case "rdd":
                    return RelatorType.RadioDirector;

                case "rpc":
                    return RelatorType.RadioProducer;

                case "rce":
                    return RelatorType.RecordingEngineer;

                case "rcd":
                    return RelatorType.Recordist;

                case "red":
                    return RelatorType.Redaktor;

                case "ren":
                    return RelatorType.Renderer;

                case "rpt":
                    return RelatorType.Reporter;

                case "rps":
                    return RelatorType.Repository;

                case "rth":
                    return RelatorType.ResearchTeamHead;

                case "rtm":
                    return RelatorType.ResearchTeamMember;

                case "res":
                    return RelatorType.Researcher;

                case "rsp":
                    return RelatorType.Respondent;

                case "rst":
                    return RelatorType.RespondentAppellant;

                case "rse":
                    return RelatorType.RespondentAppellee;

                case "rpy":
                    return RelatorType.ResponsibleParty;

                case "rsg":
                    return RelatorType.Restager;

                case "rsr":
                    return RelatorType.Restorationist;

                case "rev":
                    return RelatorType.Reviewer;

                case "rbr":
                    return RelatorType.Rubricator;

                case "sce":
                    return RelatorType.Scenarist;

                case "sad":
                    return RelatorType.ScientificAdvisor;

                case "aus":
                    return RelatorType.Screenwriter;

                case "scr":
                    return RelatorType.Scribe;

                case "scl":
                    return RelatorType.Sculptor;

                case "spy":
                    return RelatorType.SecondParty;

                case "sec":
                    return RelatorType.Secretary;

                case "sll":
                    return RelatorType.Seller;

                case "std":
                    return RelatorType.SetDesigner;

                case "stg":
                    return RelatorType.Setting;

                case "sgn":
                    return RelatorType.Signer;

                case "sng":
                    return RelatorType.Singer;

                case "sds":
                    return RelatorType.SoundDesigner;

                case "spk":
                    return RelatorType.Speaker;

                case "spn":
                    return RelatorType.Sponsor;

                case "sgd":
                    return RelatorType.StageDirector;

                case "stm":
                    return RelatorType.StageManager;

                case "stn":
                    return RelatorType.StandardsBody;

                case "str":
                    return RelatorType.Stereotyper;

                case "stl":
                    return RelatorType.Storyteller;

                case "sht":
                    return RelatorType.SupportingHost;

                case "srv":
                    return RelatorType.Surveyor;

                case "tch":
                    return RelatorType.Teacher;

                case "tcd":
                    return RelatorType.TechnicalDirector;

                case "tld":
                    return RelatorType.TelevisionDirector;

                case "tlp":
                    return RelatorType.TelevisionProducer;

                case "ths":
                    return RelatorType.ThesisAdvisor;

                case "trc":
                    return RelatorType.Transcriber;

                case "trl":
                    return RelatorType.Translator;

                case "tyd":
                    return RelatorType.TypeDesigner;

                case "tyg":
                    return RelatorType.Typographer;

                case "uvp":
                    return RelatorType.UniversityPlace;

                case "vdg":
                    return RelatorType.Videographer;

                case "vac":
                    return RelatorType.VoiceActor;

                case "wit":
                    return RelatorType.Witness;

                case "wde":
                    return RelatorType.WoodEngraver;

                case "wdc":
                    return RelatorType.Woodcutter;

                case "wam":
                    return RelatorType.WriterOfAccompanyingMaterial;

                case "wac":
                    return RelatorType.WriterOfAddedCommentary;

                case "wal":
                    return RelatorType.WriterOfAddedLyrics;

                case "wat":
                    return RelatorType.WriterOfAddedText;

                case "win":
                    return RelatorType.WriterOfIntroduction;

                case "wpr":
                    return RelatorType.WriterOfPreface;

                case "wst":
                    return RelatorType.WriterOfSupplementaryTextualContent;

                default:
                    throw new ArgumentException($"Can't parse relator type: {rel}", nameof(rel));
            }
        }
    }
}
