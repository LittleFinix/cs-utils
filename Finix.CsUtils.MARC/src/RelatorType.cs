using System;

namespace Finix.CsUtils.MARC
{

    public enum RelatorType
    {
        /// <summary>
        /// [abr] A person, family, or organization contributing to a resource by shortening or condensing the original work but leaving the nature and content of the original work substantially unchanged. For substantial modifications that result in the creation of a new work, see author
        /// </summary>
        Abridger,

        /// <summary>
        /// [act] A performer contributing to an expression of a work by acting as a cast member or player in a musical or dramatic presentation, etc.
        /// </summary>
        Actor,

        /// <summary>
        /// [adp] A person or organization who 1) reworks a musical composition, usually for a different medium, or 2) rewrites novels or stories for motion pictures or other audiovisual medium.
        /// </summary>
        Adapter,

        /// <summary>
        /// [rcp] A person, family, or organization to whom the correspondence in a work is addressed UF Recipient
        /// </summary>
        Addressee,

        /// <summary>
        /// [anl] A person or organization that reviews, examines and interprets data or information in a specific area
        /// </summary>
        Analyst,

        /// <summary>
        /// [anm] A person contributing to a moving image work or computer program by giving apparent movement to inanimate objects or drawings. For the creator of the drawings that are animated, see artist
        /// </summary>
        Animator,

        /// <summary>
        /// [ann] A person who makes manuscript annotations on an item
        /// </summary>
        Annotator,

        /// <summary>
        /// [apl] A person or organization who appeals a lower court's decision
        /// </summary>
        Appellant,

        /// <summary>
        /// [ape] A person or organization against whom an appeal is taken
        /// </summary>
        Appellee,

        /// <summary>
        /// [app] A person or organization responsible for the submission of an application or who is named as eligible for the results of the processing of the application (e.g., bestowing of rights, reward, title, position)
        /// </summary>
        Applicant,

        /// <summary>
        /// [arc] A person, family, or organization responsible for creating an architectural design, including a pictorial representation intended to show how a building, etc., will look when completed. It also oversees the construction of structures
        /// </summary>
        Architect,

        /// <summary>
        /// [arr] A person, family, or organization contributing to a musical work by rewriting the composition for a medium of performance different from that for which the work was originally intended, or modifying the work for the same medium of performance, etc., such that the musical substance of the original composition remains essentially unchanged. For extensive modification that effectively results in the creation of a new musical work, see composer UF Arranger of music
        /// </summary>
        Arranger,

        /// <summary>
        /// [acp] A person (e.g., a painter or sculptor) who makes copies of works of visual art
        /// </summary>
        ArtCopyist,

        /// <summary>
        /// [adi] A person contributing to a motion picture or television production by overseeing the artists and craftspeople who build the sets
        /// </summary>
        ArtDirector,

        /// <summary>
        /// [art] A person, family, or organization responsible for creating a work by conceiving, and implementing, an original graphic design, drawing, painting, etc. For book illustrators, prefer Illustrator [ill] UF Graphic technician
        /// </summary>
        Artist,

        /// <summary>
        /// [ard] A person responsible for controlling the development of the artistic style of an entire production, including the choice of works to be presented and selection of senior production staff
        /// </summary>
        ArtisticDirector,

        /// <summary>
        /// [asg] A person or organization to whom a license for printing or publishing has been transferred
        /// </summary>
        Assignee,

        /// <summary>
        /// [asn] A person or organization associated with or found in an item or collection, which cannot be determined to be that of a Former owner [fmo] or other designated relationship indicative of provenance
        /// </summary>
        AssociatedName,

        /// <summary>
        /// [att] An author, artist, etc., relating him/her to a resource for which there is or once was substantial authority for designating that person as author, creator, etc. of the work UF Supposed name
        /// </summary>
        AttributedName,

        /// <summary>
        /// [auc] A person or organization in charge of the estimation and public auctioning of goods, particularly books, artistic works, etc.
        /// </summary>
        Auctioneer,

        /// <summary>
        /// [aut] A person, family, or organization responsible for creating a work that is primarily textual in content, regardless of media type (e.g., printed text, spoken word, electronic text, tactile text) or genre (e.g., poems, novels, screenplays, blogs). Use also for persons, etc., creating a new work by paraphrasing, rewriting, or adapting works by another creator such that the modification has substantially changed the nature and content of the original or changed the medium of expression UF Joint author
        /// </summary>
        Author,

        /// <summary>
        /// [aqt] A person or organization whose work is largely quoted or extracted in works to which he or she did not contribute directly. Such quotations are found particularly in exhibition catalogs, collections of photographs, etc.
        /// </summary>
        AuthorInQuotationsOrTextAbstracts,

        /// <summary>
        /// [ colophon]  etc.,aft,A person or organization responsible for an afterword, postface, colophon, etc. but who is not the chief author of a work
        /// </summary>
        AuthorOfAfterword,

        /// <summary>
        /// [aud] A person or organization responsible for the dialog or spoken commentary for a screenplay or sound recording
        /// </summary>
        AuthorOfDialog,

        /// <summary>
        /// [ etc.] aui,A person or organization responsible for an introduction, preface, foreword, or other critical introductory matter, but who is not the chief author
        /// </summary>
        AuthorOfIntroduction,

        /// <summary>
        /// [ato] A person whose manuscript signature appears on an item
        /// </summary>
        Autographer,

        /// <summary>
        /// [ant] A person or organization responsible for a resource upon which the resource represented by the bibliographic description is based. This may be appropriate for adaptations, sequels, continuations, indexes, etc.
        /// </summary>
        BibliographicAntecedent,

        /// <summary>
        /// [bnd] A person who binds an item
        /// </summary>
        Binder,

        /// <summary>
        /// [bdd] A person or organization responsible for the binding design of a book, including the type of binding, the type of materials used, and any decorative aspects of the binding UF Designer of binding
        /// </summary>
        BindingDesigner,

        /// <summary>
        /// [blw] A person or organization responsible for writing a commendation or testimonial for a work, which appears on or within the publication itself, frequently on the back or dust jacket of print publications or on advertising material for all media
        /// </summary>
        BlurbWriter,

        /// <summary>
        /// [bkd] A person or organization involved in manufacturing a manifestation by being responsible for the entire graphic design of a book, including arrangement of type and illustration, choice of materials, and process used UF Designer of book UF Designer of e-book
        /// </summary>
        BookDesigner,

        /// <summary>
        /// [bkp] A person or organization responsible for the production of books and other print media UF Producer of book
        /// </summary>
        BookProducer,

        /// <summary>
        /// [bjd] A person or organization responsible for the design of flexible covers designed for or published with a book, including the type of materials used, and any decorative aspects of the bookjacket UF Designer of bookjacket
        /// </summary>
        BookjacketDesigner,

        /// <summary>
        /// [bpd] A person or organization responsible for the design of a book owner's identification label that is most commonly pasted to the inside front cover of a book
        /// </summary>
        BookplateDesigner,

        /// <summary>
        /// [bsl] A person or organization who makes books and other bibliographic materials available for purchase. Interest in the materials is primarily lucrative
        /// </summary>
        Bookseller,

        /// <summary>
        /// [brl] A person, family, or organization involved in manufacturing a resource by embossing Braille cells using a stylus, special embossing printer, or other device
        /// </summary>
        BrailleEmbosser,

        /// <summary>
        /// [brd] A person, family, or organization involved in broadcasting a resource to an audience via radio, television, webcast, etc.
        /// </summary>
        Broadcaster,

        /// <summary>
        /// [cll] A person or organization who writes in an artistic hand, usually as a copyist and or engrosser
        /// </summary>
        Calligrapher,

        /// <summary>
        /// [ctg] A person, family, or organization responsible for creating a map, atlas, globe, or other cartographic work
        /// </summary>
        Cartographer,

        /// <summary>
        /// [cas] A person, family, or organization involved in manufacturing a resource by pouring a liquid or molten substance into a mold and leaving it to solidify to take the shape of the mold
        /// </summary>
        Caster,

        /// <summary>
        /// [cns] A person or organization who examines bibliographic resources for the purpose of suppressing parts deemed objectionable on moral, political, military, or other grounds UF Bowdlerizer UF Expurgator
        /// </summary>
        Censor,

        /// <summary>
        /// [chr] A person responsible for creating or contributing to a work of movement
        /// </summary>
        Choreographer,

        /// <summary>
        /// [cng] A person in charge of photographing a motion picture, who plans the technical aspets of lighting and photographing of scenes, and often assists the director in the choice of angles, camera setups, and lighting moods. He or she may also supervise the further processing of filmed material up to the completion of the work print. Cinematographer is also referred to as director of photography. Do not confuse with videographer UF Director of photography
        /// </summary>
        Cinematographer,

        /// <summary>
        /// [cli] A person or organization for whom another person or organization is acting
        /// </summary>
        Client,

        /// <summary>
        /// [cor] A curator who lists or inventories the items in an aggregate work such as a collection of items or works
        /// </summary>
        CollectionRegistrar,

        /// <summary>
        /// [col] A curator who brings together items from various sources that are then arranged, described, and cataloged as a collection. A collector is neither the creator of the material nor a person to whom manuscripts in the collection may have been addressed
        /// </summary>
        Collector,

        /// <summary>
        /// [clt] A person, family, or organization involved in manufacturing a manifestation of photographic prints from film or other colloid that has ink-receptive and ink-repellent surfaces
        /// </summary>
        Collotyper,

        /// <summary>
        /// [clr] A person or organization responsible for applying color to drawings, prints, photographs, maps, moving images, etc
        /// </summary>
        Colorist,

        /// <summary>
        /// [cmm] A performer contributing to a work by providing interpretation, analysis, or a discussion of the subject matter on a recording, film, or other audiovisual medium
        /// </summary>
        Commentator,

        /// <summary>
        /// [cwt] A person or organization responsible for the commentary or explanatory notes about a text. For the writer of manuscript annotations in a printed book, use Annotator
        /// </summary>
        CommentatorForWrittenText,

        /// <summary>
        /// [com] A person, family, or organization responsible for creating a new work (e.g., a bibliography, a directory) through the act of compilation, e.g., selecting, arranging, aggregating, and editing data, information, etc
        /// </summary>
        Compiler,

        /// <summary>
        /// [cpl] A person or organization who applies to the courts for redress, usually in an equity proceeding
        /// </summary>
        Complainant,

        /// <summary>
        /// [cpt] A complainant who takes an appeal from one court or jurisdiction to another to reverse the judgment, usually in an equity proceeding
        /// </summary>
        ComplainantAppellant,

        /// <summary>
        /// [cpe] A complainant against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment, usually in an equity proceeding
        /// </summary>
        ComplainantAppellee,

        /// <summary>
        /// [cmp] A person, family, or organization responsible for creating or contributing to a musical resource by adding music to a work that originally lacked it or supplements it
        /// </summary>
        Composer,

        /// <summary>
        /// [cmt] A person or organization responsible for the creation of metal slug, or molds made of other materials, used to produce the text and images in printed matter UF Typesetter
        /// </summary>
        Compositor,

        /// <summary>
        /// [ccp] A person or organization responsible for the original idea on which a work is based, this includes the scientific author of an audio-visual item and the conceptor of an advertisement
        /// </summary>
        Conceptor,

        /// <summary>
        /// [cnd] A performer contributing to a musical resource by leading a performing group (orchestra, chorus, opera, etc.) in a musical or dramatic presentation, etc.
        /// </summary>
        Conductor,

        /// <summary>
        /// [con] A person or organization responsible for documenting, preserving, or treating printed or manuscript material, works of art, artifacts, or other media UF Preservationist
        /// </summary>
        Conservator,

        /// <summary>
        /// [csl] A person or organization relevant to a resource, who is called upon for professional advice or services in a specialized field of knowledge or training
        /// </summary>
        Consultant,

        /// <summary>
        /// [csp] A person or organization relevant to a resource, who is engaged specifically to provide an intellectual overview of a strategic or operational task and by analysis, specification, or instruction, to create or propose a cost-effective course of action or solution
        /// </summary>
        ConsultantToAProject,

        /// <summary>
        /// [cos] A person(s) or organization who opposes, resists, or disputes, in a court of law, a claim, decision, result, etc.
        /// </summary>
        Contestant,

        /// <summary>
        /// [cot] A contestant who takes an appeal from one court of law or jurisdiction to another to reverse the judgment
        /// </summary>
        ContestantAppellant,

        /// <summary>
        /// [coe] A contestant against whom an appeal is taken from one court of law or jurisdiction to another to reverse the judgment
        /// </summary>
        ContestantAppellee,

        /// <summary>
        /// [cts] A person(s) or organization defending a claim, decision, result, etc. being opposed, resisted, or disputed in a court of law
        /// </summary>
        Contestee,

        /// <summary>
        /// [ctt] A contestee who takes an appeal from one court or jurisdiction to another to reverse the judgment
        /// </summary>
        ContesteeAppellant,

        /// <summary>
        /// [cte] A contestee against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment
        /// </summary>
        ContesteeAppellee,

        /// <summary>
        /// [ctr] A person or organization relevant to a resource, who enters into a contract with another person or organization to perform a specific
        /// </summary>
        Contractor,

        /// <summary>
        /// [ctb] A person, family or organization responsible for making contributions to the resource. This includes those whose work has been contributed to a larger work, such as an anthology, serial publication, or other compilation of individual works. If a more specific role is available, prefer that, e.g. editor, compiler, illustrator UF Collaborator
        /// </summary>
        Contributor,

        /// <summary>
        /// [cpc] A person or organization listed as a copyright owner at the time of registration. Copyright can be granted or later transferred to another person or organization, at which time the claimant becomes the copyright holder
        /// </summary>
        CopyrightClaimant,

        /// <summary>
        /// [cph] A person or organization to whom copy and legal rights have been granted or transferred for the intellectual content of a work. The copyright holder, although not necessarily the creator of the work, usually has the exclusive right to benefit financially from the sale and use of the work to which the associated copyright protection applies
        /// </summary>
        CopyrightHolder,

        /// <summary>
        /// [crr] A person or organization who is a corrector of manuscripts, such as the scriptorium official who corrected the work of a scribe. For printed matter, use Proofreader
        /// </summary>
        Corrector,

        /// <summary>
        /// [crp] A person or organization who was either the writer or recipient of a letter or other communication
        /// </summary>
        Correspondent,

        /// <summary>
        /// [cst] A person, family, or organization that designs the costumes for a moving image production or for a musical or dramatic presentation or entertainment
        /// </summary>
        CostumeDesigner,

        /// <summary>
        /// [cou] A court governed by court rules, regardless of their official nature (e.g., laws, administrative regulations)
        /// </summary>
        CourtGoverned,

        /// <summary>
        /// [crt] A person, family, or organization contributing to a resource by preparing a court's opinions for publication
        /// </summary>
        CourtReporter,

        /// <summary>
        /// [cov] A person or organization responsible for the graphic design of a book cover, album cover, slipcase, box, container, etc. For a person or organization responsible for the graphic design of an entire book, use Book designer; for book jackets, use Bookjacket designer UF Designer of cover
        /// </summary>
        CoverDesigner,

        /// <summary>
        /// [cre] A person or organization responsible for the intellectual or artistic content of a resource
        /// </summary>
        Creator,

        /// <summary>
        /// [cur] A person, family, or organization conceiving, aggregating, and/or organizing an exhibition, collection, or other item UF Curator of an exhibition
        /// </summary>
        Curator,

        /// <summary>
        /// [dnc] A performer who dances in a musical, dramatic, etc., presentation
        /// </summary>
        Dancer,

        /// <summary>
        /// [dtc] A person or organization that submits data for inclusion in a database or other collection of data
        /// </summary>
        DataContributor,

        /// <summary>
        /// [dtm] A person or organization responsible for managing databases or other data sources
        /// </summary>
        DataManager,

        /// <summary>
        /// [dte] A person, family, or organization to whom a resource is dedicated UF Dedicatee of item
        /// </summary>
        Dedicatee,

        /// <summary>
        /// [dto] A person who writes a dedication, which may be a formal statement or in epistolary or verse form
        /// </summary>
        Dedicator,

        /// <summary>
        /// [dfd] A person or organization who is accused in a criminal proceeding or sued in a civil proceeding
        /// </summary>
        Defendant,

        /// <summary>
        /// [dft] A defendant who takes an appeal from one court or jurisdiction to another to reverse the judgment, usually in a legal action
        /// </summary>
        DefendantAppellant,

        /// <summary>
        /// [dfe] A defendant against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment, usually in a legal action
        /// </summary>
        DefendantAppellee,

        /// <summary>
        /// [dgg] A organization granting an academic degree UF Degree grantor
        /// </summary>
        DegreeGrantingInstitution,

        /// <summary>
        /// [dgs] A person overseeing a higher level academic degree
        /// </summary>
        DegreeSupervisor,

        /// <summary>
        /// [dln] A person or organization executing technical drawings from others' designs
        /// </summary>
        Delineator,

        /// <summary>
        /// [dpc] An entity depicted or portrayed in a work, particularly in a work of art
        /// </summary>
        Depicted,

        /// <summary>
        /// [dpt] A current owner of an item who deposited the item into the custody of another person, family, or organization, while still retaining ownership
        /// </summary>
        Depositor,

        /// <summary>
        /// [dsr] A person, family, or organization responsible for creating a design for an object
        /// </summary>
        Designer,

        /// <summary>
        /// [drt] A person responsible for the general management and supervision of a filmed performance, a radio or television program, etc.
        /// </summary>
        Director,

        /// <summary>
        /// [dis] A person who presents a thesis for a university or higher-level educational degree
        /// </summary>
        Dissertant,

        /// <summary>
        /// [dbp] A place from which a resource, e.g., a serial, is distributed
        /// </summary>
        DistributionPlace,

        /// <summary>
        /// [dst] A person or organization that has exclusive or shared marketing rights for a resource
        /// </summary>
        Distributor,

        /// <summary>
        /// [dnr] A former owner of an item who donated that item to another owner
        /// </summary>
        Donor,

        /// <summary>
        /// [drm] A person, family, or organization contributing to a resource by an architect, inventor, etc., by making detailed plans or drawings for buildings, ships, aircraft, machines, objects, etc UF Technical draftsman
        /// </summary>
        Draftsman,

        /// <summary>
        /// [dub] A person or organization to which authorship has been dubiously or incorrectly ascribed
        /// </summary>
        DubiousAuthor,

        /// <summary>
        /// [edt] A person, family, or organization contributing to a resource by revising or elucidating the content, e.g., adding an introduction, notes, or other critical matter. An editor may also prepare a resource for production, publication, or distribution. For major revisions, adaptations, etc., that substantially change the nature and content of the original work, resulting in a new work, see author
        /// </summary>
        Editor,

        /// <summary>
        /// [edc] A person, family, or organization contributing to a collective or aggregate work by selecting and putting together works, or parts of works, by one or more creators. For compilations of data, information, etc., that result in new works, see compiler
        /// </summary>
        EditorOfCompilation,

        /// <summary>
        /// [edm] A person, family, or organization responsible for assembling, arranging, and trimming film, video, or other moving image formats, including both visual and audio aspects UF Moving image work editor
        /// </summary>
        EditorOfMovingImageWork,

        /// <summary>
        /// [elg] A person responsible for setting up a lighting rig and focusing the lights for a production, and running the lighting at a performance UF Chief electrician UF House electrician UF Master electrician
        /// </summary>
        Electrician,

        /// <summary>
        /// [elt] A person or organization who creates a duplicate printing surface by pressure molding and electrodepositing of metal that is then backed up with lead for printing
        /// </summary>
        Electrotyper,

        /// <summary>
        /// [enj] A jurisdiction enacting a law, regulation, constitution, court rule, etc.
        /// </summary>
        EnactingJurisdiction,

        /// <summary>
        /// [eng] A person or organization that is responsible for technical planning and design, particularly with construction
        /// </summary>
        Engineer,

        /// <summary>
        /// [egr] A person or organization who cuts letters, figures, etc. on a surface, such as a wooden or metal plate used for printing
        /// </summary>
        Engraver,

        /// <summary>
        /// [etr] A person or organization who produces text or images for printing by subjecting metal, glass, or some other surface to acid or the corrosive action of some other substance
        /// </summary>
        Etcher,

        /// <summary>
        /// [evp] A place where an event such as a conference or a concert took place
        /// </summary>
        EventPlace,

        /// <summary>
        /// [exp] A person or organization in charge of the description and appraisal of the value of goods, particularly rare items, works of art, etc. UF Appraiser
        /// </summary>
        Expert,

        /// <summary>
        /// [fac] A person or organization that executed the facsimile UF Copier
        /// </summary>
        Facsimilist,

        /// <summary>
        /// [fld] A person or organization that manages or supervises the work done to collect raw data or do research in an actual setting or environment (typically applies to the natural and social sciences)
        /// </summary>
        FieldDirector,

        /// <summary>
        /// [fmd] A director responsible for the general management and supervision of a filmed performance
        /// </summary>
        FilmDirector,

        /// <summary>
        /// [fds] A person, family, or organization involved in distributing a moving image resource to theatres or other distribution channels
        /// </summary>
        FilmDistributor,

        /// <summary>
        /// [flm] A person who, following the script and in creative cooperation with the Director, selects, arranges, and assembles the filmed material, controls the synchronization of picture and sound, and participates in other post-production tasks such as sound mixing and visual effects processing. Today, picture editing is often performed digitally. UF Motion picture editor
        /// </summary>
        FilmEditor,

        /// <summary>
        /// [fmp] A producer responsible for most of the business aspects of a film
        /// </summary>
        FilmProducer,

        /// <summary>
        /// [fmk] A person, family or organization responsible for creating an independent or personal film. A filmmaker is individually responsible for the conception and execution of all aspects of the film
        /// </summary>
        Filmmaker,

        /// <summary>
        /// [fpy] A person or organization who is identified as the only party or the party of the first party. In the case of transfer of rights, this is the assignor, transferor, licensor, grantor, etc. Multiple parties can be named jointly as the first party
        /// </summary>
        FirstParty,

        /// <summary>
        /// [frg] A person or organization who makes or imitates something of value or importance, especially with the intent to defraud UF Copier UF Counterfeiter
        /// </summary>
        Forger,

        /// <summary>
        /// [fmo] A person, family, or organization formerly having legal possession of an item
        /// </summary>
        FormerOwner,

        /// <summary>
        /// [fnd] A person or organization that furnished financial support for the production of the work
        /// </summary>
        Funder,

        /// <summary>
        /// [gis] A person responsible for geographic information system (GIS) development and integration with global positioning system data UF Geospatial information specialist
        /// </summary>
        GeographicInformationSpecialist,

        /// <summary>
        /// [hnr] A person, family, or organization honored by a work or item (e.g., the honoree of a festschrift, a person to whom a copy is presented) UF Honouree UF Honouree of item
        /// </summary>
        Honoree,

        /// <summary>
        /// [hst] A performer contributing to a resource by leading a program (often broadcast) that includes other guests, performers, etc. (e.g., talk show host)
        /// </summary>
        Host,

        /// <summary>
        /// [his] An organization hosting the event, exhibit, conference, etc., which gave rise to a resource, but having little or no responsibility for the content of the resource
        /// </summary>
        HostInstitution,

        /// <summary>
        /// [ilu] A person providing decoration to a specific item using precious metals or color, often with elaborate designs and motifs
        /// </summary>
        Illuminator,

        /// <summary>
        /// [ill] A person, family, or organization contributing to a resource by supplementing the primary content with drawings, diagrams, photographs, etc. If the work is primarily the artistic content created by this entity, use artist or photographer
        /// </summary>
        Illustrator,

        /// <summary>
        /// [ins] A person who has written a statement of dedication or gift
        /// </summary>
        Inscriber,

        /// <summary>
        /// [itr] A performer contributing to a resource by playing a musical instrument
        /// </summary>
        Instrumentalist,

        /// <summary>
        /// [ive] A person, family or organization responsible for creating or contributing to a resource by responding to an interviewer, usually a reporter, pollster, or some other information gathering agent
        /// </summary>
        Interviewee,

        /// <summary>
        /// [ivr] A person, family, or organization responsible for creating or contributing to a resource by acting as an interviewer, reporter, pollster, or some other information gathering agent
        /// </summary>
        Interviewer,

        /// <summary>
        /// [inv] A person, family, or organization responsible for creating a new device or process UF Patent inventor
        /// </summary>
        Inventor,

        /// <summary>
        /// [isb] A person, family or organization issuing a work, such as an official organ of the body
        /// </summary>
        IssuingBody,

        /// <summary>
        /// [jud] A person who hears and decides on legal matters in court.
        /// </summary>
        Judge,

        /// <summary>
        /// [jug] A jurisdiction governed by a law, regulation, etc., that was enacted by another jurisdiction
        /// </summary>
        JurisdictionGoverned,

        /// <summary>
        /// [lbr] An organization that provides scientific analyses of material samples
        /// </summary>
        Laboratory,

        /// <summary>
        /// [ldr] A person or organization that manages or supervises work done in a controlled setting or environment UF Lab director
        /// </summary>
        LaboratoryDirector,

        /// <summary>
        /// [lsa] An architect responsible for creating landscape works. This work involves coordinating the arrangement of existing and proposed land features and structures
        /// </summary>
        LandscapeArchitect,

        /// <summary>
        /// [led] A person or organization that takes primary responsibility for a particular activity or endeavor. May be combined with another relator term or code to show the greater importance this person or organization has regarding that particular role. If more than one relator is assigned to a heading, use the Lead relator only if it applies to all the relators
        /// </summary>
        Lead,

        /// <summary>
        /// [len] A person or organization permitting the temporary use of a book, manuscript, etc., such as for photocopying or microfilming
        /// </summary>
        Lender,

        /// <summary>
        /// [lil] A person or organization who files a libel in an ecclesiastical or admiralty case
        /// </summary>
        Libelant,

        /// <summary>
        /// [lit] A libelant who takes an appeal from one ecclesiastical court or admiralty to another to reverse the judgment
        /// </summary>
        LibelantAppellant,

        /// <summary>
        /// [lie] A libelant against whom an appeal is taken from one ecclesiastical court or admiralty to another to reverse the judgment
        /// </summary>
        LibelantAppellee,

        /// <summary>
        /// [lel] A person or organization against whom a libel has been filed in an ecclesiastical court or admiralty
        /// </summary>
        Libelee,

        /// <summary>
        /// [let] A libelee who takes an appeal from one ecclesiastical court or admiralty to another to reverse the judgment
        /// </summary>
        LibeleeAppellant,

        /// <summary>
        /// [lee] A libelee against whom an appeal is taken from one ecclesiastical court or admiralty to another to reverse the judgment
        /// </summary>
        LibeleeAppellee,

        /// <summary>
        /// [lbt] An author of a libretto of an opera or other stage work, or an oratorio
        /// </summary>
        Librettist,

        /// <summary>
        /// [lse] A person or organization who is an original recipient of the right to print or publish
        /// </summary>
        Licensee,

        /// <summary>
        /// [lso] A person or organization who is a signer of the license, imprimatur, etc UF Imprimatur
        /// </summary>
        Licensor,

        /// <summary>
        /// [lgd] A person or organization who designs the lighting scheme for a theatrical presentation, entertainment, motion picture, etc.
        /// </summary>
        LightingDesigner,

        /// <summary>
        /// [ltg] A person or organization who prepares the stone or plate for lithographic printing, including a graphic artist creating a design directly on the surface from which printing will be done.
        /// </summary>
        Lithographer,

        /// <summary>
        /// [lyr] An author of the words of a non-dramatic musical work (e.g. the text of a song), except for oratorios
        /// </summary>
        Lyricist,

        /// <summary>
        /// [mfp] The place of manufacture (e.g., printing, duplicating, casting, etc.) of a resource in a published form
        /// </summary>
        ManufacturePlace,

        /// <summary>
        /// [mfr] A person or organization responsible for printing, duplicating, casting, etc. a resource
        /// </summary>
        Manufacturer,

        /// <summary>
        /// [mrb] The entity responsible for marbling paper, cloth, leather, etc. used in construction of a resource
        /// </summary>
        Marbler,

        /// <summary>
        /// [mrk] A person or organization performing the coding of SGML, HTML, or XML markup of metadata, text, etc. UF Encoder
        /// </summary>
        MarkupEditor,

        /// <summary>
        /// [med] A person held to be a channel of communication between the earthly world and a world
        /// </summary>
        Medium,

        /// <summary>
        /// [mdc] A person or organization primarily responsible for compiling and maintaining the original description of a metadata set (e.g., geospatial metadata set)
        /// </summary>
        MetadataContact,

        /// <summary>
        /// [mte] An engraver responsible for decorations, illustrations, letters, etc. cut on a metal surface for printing or decoration
        /// </summary>
        MetalEngraver,

        /// <summary>
        /// [mtk] A person, family, or organization responsible for recording the minutes of a meeting
        /// </summary>
        MinuteTaker,

        /// <summary>
        /// [mod] A performer contributing to a resource by leading a program (often broadcast) where topics are discussed, usually with participation of experts in fields related to the discussion
        /// </summary>
        Moderator,

        /// <summary>
        /// [mon] A person or organization that supervises compliance with the contract and is responsible for the report and controls its distribution. Sometimes referred to as the grantee, or controlling agency
        /// </summary>
        Monitor,

        /// <summary>
        /// [mcp] A person who transcribes or copies musical notation
        /// </summary>
        MusicCopyist,

        /// <summary>
        /// [msd] A person who coordinates the activities of the composer, the sound editor, and sound mixers for a moving image production or for a musical or dramatic presentation or entertainment
        /// </summary>
        MusicalDirector,

        /// <summary>
        /// [mus] A person or organization who performs music or contributes to the musical content of a work when it is not possible or desirable to identify the function more precisely
        /// </summary>
        Musician,

        /// <summary>
        /// [nrt] A performer contributing to a resource by reading or speaking in order to give an account of an act, occurrence, course of events, etc
        /// </summary>
        Narrator,

        /// <summary>
        /// [osp] A performer contributing to an expression of a work by appearing on screen in nonfiction moving image materials or introductions to fiction moving image materials to provide contextual or background information. Use when another term (e.g., narrator, host) is either not applicable or not desired
        /// </summary>
        OnscreenPresenter,

        /// <summary>
        /// [opn] A person or organization responsible for opposing a thesis or dissertation
        /// </summary>
        Opponent,

        /// <summary>
        /// [orm] A person, family, or organization organizing the exhibit, event, conference, etc., which gave rise to a resource UF Organizer of meeting
        /// </summary>
        Organizer,

        /// <summary>
        /// [org] A person or organization performing the work, i.e., the name of a person or organization associated with the intellectual content of the work. This category does not include the publisher or personal affiliation, or sponsor except where it is also the corporate author
        /// </summary>
        Originator,

        /// <summary>
        /// [oth] A role that has no equivalent in the MARC list.
        /// </summary>
        Other,

        /// <summary>
        /// [own] A person, family, or organization that currently owns an item or collection, i.e.has legal possession of a resource UF Current owner
        /// </summary>
        Owner,

        /// <summary>
        /// [pan] A performer contributing to a resource by participating in a program (often broadcast) where topics are discussed, usually with participation of experts in fields related to the discussion
        /// </summary>
        Panelist,

        /// <summary>
        /// [ppm] A person or organization responsible for the production of paper, usually from wood, cloth, or other fibrous material
        /// </summary>
        Papermaker,

        /// <summary>
        /// [pta] A person or organization that applied for a patent
        /// </summary>
        PatentApplicant,

        /// <summary>
        /// [pth] A person or organization that was granted the patent referred to by the item UF Patentee
        /// </summary>
        PatentHolder,

        /// <summary>
        /// [pat] A person or organization responsible for commissioning a work. Usually a patron uses his or her means or influence to support the work of artists, writers, etc. This includes those who commission and pay for individual works
        /// </summary>
        Patron,

        /// <summary>
        /// [prf] A person contributing to a resource by performing music, acting, dancing, speaking, etc., often in a musical or dramatic presentation, etc. If specific codes are used, [prf] is used for a person whose principal skill is not known or specified
        /// </summary>
        Performer,

        /// <summary>
        /// [pma] An organization (usually a government agency) that issues permits under which work is accomplished
        /// </summary>
        PermittingAgency,

        /// <summary>
        /// [pht] A person, family, or organization responsible for creating a photographic work
        /// </summary>
        Photographer,

        /// <summary>
        /// [ptf] A person or organization who brings a suit in a civil proceeding
        /// </summary>
        Plaintiff,

        /// <summary>
        /// [ptt] A plaintiff who takes an appeal from one court or jurisdiction to another to reverse the judgment, usually in a legal proceeding
        /// </summary>
        PlaintiffAppellant,

        /// <summary>
        /// [pte] A plaintiff against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment, usually in a legal proceeding
        /// </summary>
        PlaintiffAppellee,

        /// <summary>
        /// [plt] A person, family, or organization involved in manufacturing a manifestation by preparing plates used in the production of printed images and/or text
        /// </summary>
        Platemaker,

        /// <summary>
        /// [pra] A person who is the faculty moderator of an academic disputation, normally proposing a thesis and participating in the ensuing disputation
        /// </summary>
        Praeses,

        /// <summary>
        /// [pre] A person or organization mentioned in an “X presents” credit for moving image materials and who is associated with production, finance, or distribution in some way. A vanity credit; in early years, normally the head of a studio
        /// </summary>
        Presenter,

        /// <summary>
        /// [prt] A person, family, or organization involved in manufacturing a manifestation of printed text, notated music, etc., from type or plates, such as a book, newspaper, magazine, broadside, score, etc
        /// </summary>
        Printer,

        /// <summary>
        /// [pop] A person or organization who prints illustrations from plates. UF Plates, printer of
        /// </summary>
        PrinterOfPlates,

        /// <summary>
        /// [prm] A person or organization who makes a relief, intaglio, or planographic printing surface
        /// </summary>
        Printmaker,

        /// <summary>
        /// [prc] A person or organization primarily responsible for performing or initiating a process, such as is done with the collection of metadata sets
        /// </summary>
        ProcessContact,

        /// <summary>
        /// [pro] A person, family, or organization responsible for most of the business aspects of a production for screen, audio recording, television, webcast, etc. The producer is generally responsible for fund raising, managing the production, hiring key personnel, arranging for distributors, etc.
        /// </summary>
        Producer,

        /// <summary>
        /// [prn] An organization that is responsible for financial, technical, and organizational management of a production for stage, screen, audio recording, television, webcast, etc.
        /// </summary>
        ProductionCompany,

        /// <summary>
        /// [prs] A person or organization responsible for designing the overall visual appearance of a moving image production
        /// </summary>
        ProductionDesigner,

        /// <summary>
        /// [pmn] A person responsible for all technical and business matters in a production
        /// </summary>
        ProductionManager,

        /// <summary>
        /// [prd] A person or organization associated with the production (props, lighting, special effects, etc.) of a musical or dramatic presentation or entertainment
        /// </summary>
        ProductionPersonnel,

        /// <summary>
        /// [prp] The place of production (e.g., inscription, fabrication, construction, etc.) of a resource in an unpublished form
        /// </summary>
        ProductionPlace,

        /// <summary>
        /// [prg] A person, family, or organization responsible for creating a computer program
        /// </summary>
        Programmer,

        /// <summary>
        /// [pdr] A person or organization with primary responsibility for all essential aspects of a project, has overall responsibility for managing projects, or provides overall direction to a project manager
        /// </summary>
        ProjectDirector,

        /// <summary>
        /// [pfr] A person who corrects printed matter. For manuscripts, use Corrector [crr]
        /// </summary>
        Proofreader,

        /// <summary>
        /// [prv] A person or organization who produces, publishes, manufactures, or distributes a resource if specific codes are not desired (e.g. [mfr], [pbl])
        /// </summary>
        Provider,

        /// <summary>
        /// [pup] The place where a resource is published
        /// </summary>
        PublicationPlace,

        /// <summary>
        /// [pbl] A person or organization responsible for publishing, releasing, or issuing a resource
        /// </summary>
        Publisher,

        /// <summary>
        /// [pbd] A person or organization who presides over the elaboration of a collective work to ensure its coherence or continuity. This includes editors-in-chief, literary editors, editors of series, etc.
        /// </summary>
        PublishingDirector,

        /// <summary>
        /// [ppt] A performer contributing to a resource by manipulating, controlling, or directing puppets or marionettes in a moving image production or a musical or dramatic presentation or entertainment
        /// </summary>
        Puppeteer,

        /// <summary>
        /// [rdd] A director responsible for the general management and supervision of a radio program
        /// </summary>
        RadioDirector,

        /// <summary>
        /// [rpc] A producer responsible for most of the business aspects of a radio program
        /// </summary>
        RadioProducer,

        /// <summary>
        /// [rce] A person contributing to a resource by supervising the technical aspects of a sound or video recording session
        /// </summary>
        RecordingEngineer,

        /// <summary>
        /// [rcd] A person or organization who uses a recording device to capture sounds and/or video during a recording session, including field recordings of natural sounds, folkloric events, music, etc.
        /// </summary>
        Recordist,

        /// <summary>
        /// [red] A person or organization who writes or develops the framework for an item without being intellectually responsible for its content
        /// </summary>
        Redaktor,

        /// <summary>
        /// [ren] A person or organization who prepares drawings of architectural designs (i.e., renderings) in accurate, representational perspective to show what the project will look like when completed
        /// </summary>
        Renderer,

        /// <summary>
        /// [rpt] A person or organization who writes or presents reports of news or current events on air or in print
        /// </summary>
        Reporter,

        /// <summary>
        /// [rps] An organization that hosts data or material culture objects and provides services to promote long term, consistent and shared use of those data or objects
        /// </summary>
        Repository,

        /// <summary>
        /// [rth] A person who directed or managed a research project
        /// </summary>
        ResearchTeamHead,

        /// <summary>
        /// [rtm] A person who participated in a research project but whose role did not involve direction or management of it
        /// </summary>
        ResearchTeamMember,

        /// <summary>
        /// [res] A person or organization responsible for performing research UF Performer of research
        /// </summary>
        Researcher,

        /// <summary>
        /// [rsp] A person or organization who makes an answer to the courts pursuant to an application for redress (usually in an equity proceeding) or a candidate for a degree who defends or opposes a thesis provided by the praeses in an academic disputation
        /// </summary>
        Respondent,

        /// <summary>
        /// [rst] A respondent who takes an appeal from one court or jurisdiction to another to reverse the judgment, usually in an equity proceeding
        /// </summary>
        RespondentAppellant,

        /// <summary>
        /// [rse] A respondent against whom an appeal is taken from one court or jurisdiction to another to reverse the judgment, usually in an equity proceeding
        /// </summary>
        RespondentAppellee,

        /// <summary>
        /// [rpy] A person or organization legally responsible for the content of the published material
        /// </summary>
        ResponsibleParty,

        /// <summary>
        /// [rsg] A person or organization, other than the original choreographer or director, responsible for restaging a choreographic or dramatic work and who contributes minimal new content
        /// </summary>
        Restager,

        /// <summary>
        /// [rsr] A person, family, or organization responsible for the set of technical, editorial, and intellectual procedures aimed at compensating for the degradation of an item by bringing it back to a state as close as possible to its original condition
        /// </summary>
        Restorationist,

        /// <summary>
        /// [rev] A person or organization responsible for the review of a book, motion picture, performance, etc.
        /// </summary>
        Reviewer,

        /// <summary>
        /// [rbr] A person or organization responsible for parts of a work, often headings or opening parts of a manuscript, that appear in a distinctive color, usually red
        /// </summary>
        Rubricator,

        /// <summary>
        /// [sce] A person or organization who is the author of a motion picture screenplay, generally the person who wrote the scenarios for a motion picture during the silent era
        /// </summary>
        Scenarist,

        /// <summary>
        /// [sad] A person or organization who brings scientific, pedagogical, or historical competence to the conception and realization on a work, particularly in the case of audio-visual items
        /// </summary>
        ScientificAdvisor,

        /// <summary>
        /// [aus] An author of a screenplay, script, or scene UF Author of screenplay, etc.
        /// </summary>
        Screenwriter,

        /// <summary>
        /// [scr] A person who is an amanuensis and for a writer of manuscripts proper. For a person who makes pen-facsimiles, use Facsimilist [fac]
        /// </summary>
        Scribe,

        /// <summary>
        /// [scl] An artist responsible for creating a three-dimensional work by modeling, carving, or similar technique
        /// </summary>
        Sculptor,

        /// <summary>
        /// [spy] A person or organization who is identified as the party of the second part. In the case of transfer of right, this is the assignee, transferee, licensee, grantee, etc. Multiple parties can be named jointly as the second party
        /// </summary>
        SecondParty,

        /// <summary>
        /// [sec] A person or organization who is a recorder, redactor, or other person responsible for expressing the views of a organization
        /// </summary>
        Secretary,

        /// <summary>
        /// [sll] A former owner of an item who sold that item to another owner
        /// </summary>
        Seller,

        /// <summary>
        /// [std] A person who translates the rough sketches of the art director into actual architectural structures for a theatrical presentation, entertainment, motion picture, etc. Set designers draw the detailed guides and specifications for building the set
        /// </summary>
        SetDesigner,

        /// <summary>
        /// [stg] An entity in which the activity or plot of a work takes place, e.g. a geographic place, a time period, a building, an event
        /// </summary>
        Setting,

        /// <summary>
        /// [sgn] A person whose signature appears without a presentation or other statement indicative of provenance. When there is a presentation statement, use Inscriber [ins].
        /// </summary>
        Signer,

        /// <summary>
        /// [sng] A performer contributing to a resource by using his/her/their voice, with or without instrumental accompaniment, to produce music. A singer's performance may or may not include actual words UF Vocalist
        /// </summary>
        Singer,

        /// <summary>
        /// [sds] A person who produces and reproduces the sound score (both live and recorded), the installation of microphones, the setting of sound levels, and the coordination of sources of sound for a production
        /// </summary>
        SoundDesigner,

        /// <summary>
        /// [spk] A performer contributing to a resource by speaking words, such as a lecture, speech, etc.
        /// </summary>
        Speaker,

        /// <summary>
        /// [spn] A person, family, or organization sponsoring some aspect of a resource, e.g., funding research, sponsoring an event UF Sponsoring body
        /// </summary>
        Sponsor,

        /// <summary>
        /// [sgd] A person or organization contributing to a stage resource through the overall management and supervision of a performance
        /// </summary>
        StageDirector,

        /// <summary>
        /// [stm] A person who is in charge of everything that occurs on a performance stage, and who acts as chief of all crews and assistant to a director during rehearsals
        /// </summary>
        StageManager,

        /// <summary>
        /// [stn] An organization responsible for the development or enforcement of a standard
        /// </summary>
        StandardsBody,

        /// <summary>
        /// [str] A person or organization who creates a new plate for printing by molding or copying another printing surface
        /// </summary>
        Stereotyper,

        /// <summary>
        /// [stl] A performer contributing to a resource by relaying a creator's original story with dramatic or theatrical interpretation
        /// </summary>
        Storyteller,

        /// <summary>
        /// [sht] A person or organization that supports (by allocating facilities, staff, or other resources) a project, program, meeting, event, data objects, material culture objects, or other entities capable of support UF Host, supporting
        /// </summary>
        SupportingHost,

        /// <summary>
        /// [srv] A person, family, or organization contributing to a cartographic resource by providing measurements or dimensional relationships for the geographic area represented
        /// </summary>
        Surveyor,

        /// <summary>
        /// [tch] A performer contributing to a resource by giving instruction or providing a demonstration UF Instructor
        /// </summary>
        Teacher,

        /// <summary>
        /// [tcd] A person who is ultimately in charge of scenery, props, lights and sound for a production
        /// </summary>
        TechnicalDirector,

        /// <summary>
        /// [tld] A director responsible for the general management and supervision of a television program
        /// </summary>
        TelevisionDirector,

        /// <summary>
        /// [tlp] A producer responsible for most of the business aspects of a television program
        /// </summary>
        TelevisionProducer,

        /// <summary>
        /// [ths] A person under whose supervision a degree candidate develops and presents a thesis, mémoire, or text of a dissertation UF Promoter
        /// </summary>
        ThesisAdvisor,

        /// <summary>
        /// [trc] A person, family, or organization contributing to a resource by changing it from one system of notation to another. For a work transcribed for a different instrument or performing group, see Arranger [arr]. For makers of pen-facsimiles, use Facsimilist [fac]
        /// </summary>
        Transcriber,

        /// <summary>
        /// [trl] A person or organization who renders a text from one language into another, or from an older form of a language into the modern form
        /// </summary>
        Translator,

        /// <summary>
        /// [tyd] A person or organization who designs the type face used in a particular item UF Designer of type
        /// </summary>
        TypeDesigner,

        /// <summary>
        /// [tyg] A person or organization primarily responsible for choice and arrangement of type used in an item. If the typographer is also responsible for other aspects of the graphic design of a book (e.g., Book designer [bkd]), codes for both functions may be needed
        /// </summary>
        Typographer,

        /// <summary>
        /// [uvp] A place where a university that is associated with a resource is located, for example, a university where an academic dissertation or thesis was presented
        /// </summary>
        UniversityPlace,

        /// <summary>
        /// [vdg] A person in charge of a video production, e.g. the video recording of a stage production as opposed to a commercial motion picture. The videographer may be the camera operator or may supervise one or more camera operators. Do not confuse with cinematographer
        /// </summary>
        Videographer,

        /// <summary>
        /// [vac] An actor contributing to a resource by providing the voice for characters in radio and audio productions and for animated characters in moving image works, as well as by providing voice overs in radio and television commercials, dubbed resources, etc.
        /// </summary>
        VoiceActor,

        /// <summary>
        /// [wit] Use for a person who verifies the truthfulness of an event or action. UF Deponent UF Eyewitness UF Observer UF Onlooker UF Testifier
        /// </summary>
        Witness,

        /// <summary>
        /// [wde] A person or organization who makes prints by cutting the image in relief on the end-grain of a wood block
        /// </summary>
        WoodEngraver,

        /// <summary>
        /// [wdc] A person or organization who makes prints by cutting the image in relief on the plank side of a wood block
        /// </summary>
        Woodcutter,

        /// <summary>
        /// [wam] A person or organization who writes significant material which accompanies a sound recording or other audiovisual material
        /// </summary>
        WriterOfAccompanyingMaterial,

        /// <summary>
        /// [wac] A person, family, or organization contributing to an expression of a work by providing an interpretation or critical explanation of the original work
        /// </summary>
        WriterOfAddedCommentary,

        /// <summary>
        /// [wal] A writer of words added to an expression of a musical work. For lyric writing in collaboration with a composer to form an original work, see lyricist
        /// </summary>
        WriterOfAddedLyrics,

        /// <summary>
        /// [wat] A person, family, or organization contributing to a non-textual resource by providing text for the non-textual work (e.g., writing captions for photographs, descriptions of maps).
        /// </summary>
        WriterOfAddedText,

        /// <summary>
        /// [win] A person, family, or organization contributing to a resource by providing an introduction to the original work
        /// </summary>
        WriterOfIntroduction,

        /// <summary>
        /// [wpr] A person, family, or organization contributing to a resource by providing a preface to the original work
        /// </summary>
        WriterOfPreface,

        /// <summary>
        /// [wst] A person, family, or organization contributing to a resource by providing supplementary textual content (e.g., an introduction, a preface) to the original work
        /// </summary>
        WriterOfSupplementaryTextualContent,


    }

    public static class Relator
    {
        public static string ToString(RelatorType rel)
        {
            switch (rel)
            {
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

                case RelatorType.Expert:
                    return "exp";

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

                case "exp":
                    return RelatorType.Expert;

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
