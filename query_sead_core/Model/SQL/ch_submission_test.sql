INSERT INTO clearing_house.tbl_clearinghouse_submissions(
            submission_id, submission_state_id, data_types, upload_user_id, 
            upload_date, upload_content, xml, status_text, claim_user_id, 
            claim_date_time)
    SELECT 2, 2, 'Undefined other', 4, now(), null, xmldata, 'new', null,  null
    FROM clearing_house.tbl_clearinghouse_xml_temp
    WHERE id = 3
update clearing_house.tbl_clearinghouse_submissions set data_types='Undefined other;Abundance', upload_content=null
select * from clearing_house.view_data_types
--          
-- ALTER TABLE clearing_house.tbl_clearinghouse_submissions DROP CONSTRAINT fk_tbl_submissions_state_id_state_id 
--   ADD CONSTRAINT fk_tbl_submissions_state_id_state_id FOREIGN KEY (submission_state_id)
--       REFERENCES clearing_house.tbl_clearinghouse_submission_states (submission_state_id) MATCH SIMPLE
--       ON UPDATE NO ACTION ON DELETE NO ACTION;

insert into clearing_house.tbl_clearinghouse_submission_states values(10, 'test')
delete from clearing_house.tbl_clearinghouse_submission_states where submission_state_id = 10
UPDATE clearing_house.tbl_clearinghouse_submissions SET submission_state_id=1, XML = '<?xml version="1.0" ?>
<sead-data-upload>
<TblMethodGroups length="4">
    <com.sead.database.TblMethodGroups id="17" cloneId="17"/>
    <com.sead.database.TblMethodGroups id="13" cloneId="13"/>
    <com.sead.database.TblMethodGroups id="14" cloneId="14"/>
    <com.sead.database.TblMethodGroups id="9" cloneId="9"/>
</TblMethodGroups>

<TblDataTypeGroups length="7">
    <com.sead.database.TblDataTypeGroups id="1" cloneId="1"/>
    <com.sead.database.TblDataTypeGroups id="2" cloneId="2"/>
    <com.sead.database.TblDataTypeGroups id="3" cloneId="3"/>
    <com.sead.database.TblDataTypeGroups id="4" cloneId="4"/>
    <com.sead.database.TblDataTypeGroups id="5" cloneId="5"/>
    <com.sead.database.TblDataTypeGroups id="7" cloneId="7"/>
    <com.sead.database.TblDataTypeGroups id="8" cloneId="8"/>
</TblDataTypeGroups>

<TblSampleDescriptionTypes length="3">
    <com.sead.database.TblSampleDescriptionTypes id="1" cloneId="1"/>
    <com.sead.database.TblSampleDescriptionTypes id="2" cloneId="2"/>
    <com.sead.database.TblSampleDescriptionTypes id="3" cloneId="3"/>
</TblSampleDescriptionTypes>

<TblSampleGroupSamplingContexts length="14">
    <com.sead.database.TblSampleGroupSamplingContexts id="1" cloneId="1"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="2" cloneId="2"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="3" cloneId="3"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="4" cloneId="4"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="5" cloneId="5"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="6" cloneId="6"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="7" cloneId="7"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="8" cloneId="8"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="9" cloneId="9"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="10" cloneId="10"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="11" cloneId="11"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="12" cloneId="12"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="13" cloneId="13"/>
    <com.sead.database.TblSampleGroupSamplingContexts id="14" cloneId="14"/>
</TblSampleGroupSamplingContexts>

<TblUnits length="6">
    <com.sead.database.TblUnits id="1" cloneId="1"/>
    <com.sead.database.TblUnits id="2" cloneId="2"/>
    <com.sead.database.TblUnits id="3" cloneId="3"/>
    <com.sead.database.TblUnits id="4" cloneId="4"/>
    <com.sead.database.TblUnits id="5" cloneId="5"/>
    <com.sead.database.TblUnits id="9" cloneId="9"/>
</TblUnits>

<TblDatingUncertainty length="2">
    <com.sead.database.TblDatingUncertainty id="6" cloneId="6"/>
    <com.sead.database.TblDatingUncertainty id="7" cloneId="7"/>
</TblDatingUncertainty>

<TblLocationTypes length="7">
    <com.sead.database.TblLocationTypes id="16" cloneId="16"/>
    <com.sead.database.TblLocationTypes id="17" cloneId="17"/>
    <com.sead.database.TblLocationTypes id="2" cloneId="2"/>
    <com.sead.database.TblLocationTypes id="18" cloneId="18"/>
    <com.sead.database.TblLocationTypes id="4" cloneId="4"/>
    <com.sead.database.TblLocationTypes id="7" cloneId="7"/>
    <com.sead.database.TblLocationTypes id="14" cloneId="14"/>
</TblLocationTypes>

  <TblAltRefTypes length="11">
    <com.sead.database.TblAltRefTypes id="1" cloneId="1"/>
    <com.sead.database.TblAltRefTypes id="2" cloneId="2"/>
    <com.sead.database.TblAltRefTypes id="3" cloneId="3"/>
    <com.sead.database.TblAltRefTypes id="4" cloneId="4"/>
    <com.sead.database.TblAltRefTypes id="5" cloneId="5"/>
    <com.sead.database.TblAltRefTypes id="6" cloneId="6"/>
    <com.sead.database.TblAltRefTypes id="7" cloneId="7"/>
    <com.sead.database.TblAltRefTypes id="8" cloneId="8"/>
    <com.sead.database.TblAltRefTypes id="9">
      <altRefType class="java.lang.String">Museum subnumber</altRefType>
      <altRefTypeId class="java.lang.Integer">NULL</altRefTypeId>
      <description class="java.lang.Character">Local identification used by a museum.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAltRefTypes>
    <com.sead.database.TblAltRefTypes id="10">
      <altRefType class="java.lang.String">Find number</altRefType>
      <altRefTypeId class="java.lang.Integer">NULL</altRefTypeId>
      <description class="java.lang.Character">Identification used in the field for artifacts</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAltRefTypes>
    <com.sead.database.TblAltRefTypes id="11">
      <altRefType class="java.lang.String">Compound ID</altRefType>
      <altRefTypeId class="java.lang.Integer">NULL</altRefTypeId>
      <description class="java.lang.Character">NULL</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAltRefTypes>
  </TblAltRefTypes>
  <TblAnalysisEntities length="11076">
    <com.sead.database.TblAnalysisEntities id="99042">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3024"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32800"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99043">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3025"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32801"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99044">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3026"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32802"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99045">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3027"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32803"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99046">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3028"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32804"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99047">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3029"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32805"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99048">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3030"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32806"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99049">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3031"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32807"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99050">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3032"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32808"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99051">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3033"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32809"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99052">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3034"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32810"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
    <com.sead.database.TblAnalysisEntities id="99053">
      <analysisEntityId class="java.lang.Integer">NULL</analysisEntityId>
      <datasetId class="com.sead.database.TblDatasets" id="3035"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32811"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblAnalysisEntities>
  </TblAnalysisEntities>
  <TblBiblio length="377">
    <com.sead.database.TblBiblio id="5576">
      <author class="java.lang.String">Strömberg, Bo. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Arkeologisk slutundersökning, En järnproduktionsplats från 1500-1600-tal i Östra Spång, Område V24:1, Örkelljunga-länsgränsen, E4-projektet i norra Skåne, Skåne, Örkelljunga socken, Östra Spång 6:1, RAÄ 6:1, UV Syd daff rapport 2004:13, RAÄ dnr 321-182-2006, Lund</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2004</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5577">
      <author class="java.lang.String">Ericson, Tyra med bidrag av Anne Carlie. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Arkeologisk undersökning 2005. Västervång - en boplats från äldre och yngre järnålder. Skåne, Trelleborgs kommun, Trelleborgs stad och socken, Västervång 3:14 m. fl. UV Syd, dokumentation av fältarbetsfasen DAFF 2006:1. Lund

</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2006</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5578">
      <author class="java.lang.String"> Granlund,  Susanne med bidrag av Leif Karlenby. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Arkeologisk undersökning. Lilla Ulvgryt. Hus och hägnad vid Hummelbäcken. E18, Örebro-Lekhyttan. Närke, Vintrosa socken, Lilla Ulvgryt 1:1, RAÄ 97. Riksantikvarieämbetet UV Bergslagen Rapport 2006:11. Örebro</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2006</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5579">
      <author class="java.lang.String"> Edlund, Martin. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Arkeologisk undersökning. Treuddar och hägnader. E18-undersökning vid Bengtstorps gravfält. Närke, Täby socken, Bengtstorp 1:1, RAÄ 34, RAÄ 35. Riksantikvarieämbetet UV Bergslagen Rapport 2008:4.Örebro</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2008</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5580">
      <author class="java.lang.String"> Hanlon, Conleth, med bidrag av Tony Björk och Björn Nilsson. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Arkeologisk undersökning. Årups Norrevång. Huslämning och härdanläggningsområde från bronsålder Skåne, Ivetofta socken, fastighet Årup 1:1, RAÄ 162. UV Syd, dokumentation av fältarbetsfasen 2003:4, Regionmuseet Kristianstad/Landsantikvarien i Skåne Rapport 2003:38. Lund och Kristianstad.</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2003</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5581">
      <author class="java.lang.String">Cullberg, K. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Ekehögen und Valtersberg  zwei Gräberfelder der vorrömischen Eisenzeit in Westschweden. Katalog. Studier i nordisk arkeologi, 11. Göteborg</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">1973</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5582">
      <author class="java.lang.String">Cademar Nilsson, Åsa, Ericson Lagerås, Karin, </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Gravfältet vid Annelöv. Ett gravfält från bronsålder och boplatslämningar från senneolitikum till äldre järnålder. Skåne, Annelöv sn, Annelöv 38:1, VKB SU13. Arkeologisk undersökning, UV Syd Rapport 1999:104.</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">1999</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5583">
      <author class="java.lang.String">Heimann, Curry. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> Hästholmen - en stenåldersboplats i Flötefjorden, Stora Le  arkeologisk undersökning år 2000 av fornlämning RAÄ nr 197, Holmedal socken, Årjängs kommun, Värmlands län . GOTARC. Serie D, Arkeologiska rapporter, 53 . Göteborgs universitet. </title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2002</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5584">
      <author class="java.lang.String">Söderberg, Bengt (red.) </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String"> UV Syd Rapport 2002:16. Arkeologisk undersökning. Järrestad i centrum. Väg 11, sträckan Östra Tommarp-Simrishamn. Järrestads sn, Skåne. RAÄ UV Syd Rapport 2002:16. Lund</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">2002</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5585">
      <author class="java.lang.String">Larsson, Lars. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String">A causewayed enclosure and a site with Valby pottery at Stävie, Western Scania. I: Meddelanden från Lunds universitets historiska museum. 1981-2. Lund</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">1982</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5586">
      <author class="java.lang.String">Hulthén, B. &amp; Welinder, S.</author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String">A Stone Age economy. Theses and papers in Nort-European Archaeology 11.  Stockholm-Lund.</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">1981</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
    <com.sead.database.TblBiblio id="5587">
      <author class="java.lang.String">Stjernquist, B. </author>
      <biblioId class="java.lang.Integer">NULL</biblioId>
      <biblioKeywordId class="TblBiblioKeywords">NULL</biblioKeywordId>
      <bugsAuthor class="java.lang.String">NULL</bugsAuthor>
      <bugsBiblioId class="TblBugsBiblio">NULL</bugsBiblioId>
      <bugsReference class="java.lang.String">NULL</bugsReference>
      <bugsTitle class="java.lang.String">NULL</bugsTitle>
      <doi class="java.lang.String">NULL</doi>
      <edition class="java.lang.String">NULL</edition>
      <isbn class="java.lang.String">NULL</isbn>
      <keywords class="java.lang.String">NULL</keywords>
      <notes class="java.lang.Character">NULL</notes>
      <number class="java.lang.String">NULL</number>
      <pages class="java.lang.String">NULL</pages>
      <pdfLink class="java.lang.String">NULL</pdfLink>
      <title class="java.lang.String">A Votive Deposit from Arlösa, Southern Sweden. In Honorem Evert Baudou. Archaeology and Environment 4. Dept. of Archaeology. Umeå.</title>
      <volume class="java.lang.String">NULL</volume>
      <year class="java.lang.Integer">1985</year>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblBiblio>
  </TblBiblio>
  <TblCeramics length="77974">
    <com.sead.database.TblCeramics id="1">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">1</ceramicsLookupId>
      <measurementValue class="java.lang.String">2</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="2">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">3</ceramicsLookupId>
      <measurementValue class="java.lang.String">1</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="3">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">4</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="4">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">2</ceramicsLookupId>
      <measurementValue class="java.lang.String">SORTERAD</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="5">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">5</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="6">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">6</ceramicsLookupId>
      <measurementValue class="java.lang.String">3</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="7">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">7</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="8">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">8</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="9">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">9</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="10">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">10</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="11">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">11</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
    <com.sead.database.TblCeramics id="12">
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <ceramicsId class="java.lang.Integer">NULL</ceramicsId>
      <ceramicsLookupId class="TblCeramicsLookup">12</ceramicsLookupId>
      <measurementValue class="java.lang.String">0</measurementValue>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramics>
  </TblCeramics>
  <TblCeramicsLookup length="28">
    <com.sead.database.TblCeramicsLookup id="1">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Fraction</name>
      <description class="java.lang.Character">Estimate of the fraction size in the clay of a ceramic. Measured on a scale of 0 - 5</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="2">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Sorting</name>
      <description class="java.lang.Character">Whether the clay in a ceramic object is sorted or unsorted.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="3">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Silt</name>
      <description class="java.lang.Character">Estimate of the amount of silt fraction material in a clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="4">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Sand</name>
      <description class="java.lang.Character">Estimate of the amount of sand fraction material in the clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="5">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Calcarbon</name>
      <description class="java.lang.Character">Estimate of the amount of Calcium carbonate in the clay of a cerami object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="6">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Ironoxide</name>
      <description class="java.lang.Character">Estimate of the amount of ironoxide in the clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="7">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Limonite</name>
      <description class="java.lang.Character">Estimate of the amount of limonite in the clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="8">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Mica</name>
      <description class="java.lang.Character">Estimate of the amount of mica in the clay of a ceramic object. Measured on a scale of 0 - 5. </description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="9">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Fossil</name>
      <description class="java.lang.Character">Estimate of the amount of fossils in the clay of a ceramic object. Measured on a scale of 0 - 5. </description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="10">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Spongie</name>
      <description class="java.lang.Character">Estimate of the amount of spongie in the clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="11">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Diatome</name>
      <description class="java.lang.Character">Estimate of the amount of diatomes in the clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
    <com.sead.database.TblCeramicsLookup id="12">
      <ceramicsLookupId class="java.lang.Integer">NULL</ceramicsLookupId>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <name class="java.lang.String">Clay: Organic</name>
      <description class="java.lang.Character">Estimate of the amount of organic material in the clay of a ceramic object. Measured on a scale of 0 - 5.</description>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblCeramicsLookup>
  </TblCeramicsLookup>
  <TblContacts length="12">
    <com.sead.database.TblContacts id="1" cloneId="1"/>
    <com.sead.database.TblContacts id="2">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">anders.lindahl@geo.lu.se</email>
      <firstName class="java.lang.String">Anders</firstName>
      <lastName class="java.lang.String">Lindahl</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">http://www.geology.lu.se/anders-lindahl</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="3">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Birgitta </firstName>
      <lastName class="java.lang.String">Hulthén</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="4">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Emma</firstName>
      <lastName class="java.lang.String">Grönberg</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="5">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Emma </firstName>
      <lastName class="java.lang.String">Ramstedt</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="6">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Paul</firstName>
      <lastName class="java.lang.String">Eklöv Petterson</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="7">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Ole</firstName>
      <lastName class="java.lang.String">Stilborg</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="8">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Siv</firstName>
      <lastName class="java.lang.String">Olsson</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="9">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Torbjörn</firstName>
      <lastName class="java.lang.String">Brosson</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">NULL</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="10">
      <address1 class="java.lang.String">Gävleborgs County Administration Board</address1>
      <address2 class="java.lang.String">Gävleborgs län</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">NULL</email>
      <firstName class="java.lang.String">Thomas</firstName>
      <lastName class="java.lang.String">Eriksson</lastName>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">http://www.lansstyrelsen.se/Gavleborg/En/Pages/default.aspx</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="11">
      <address1 class="java.lang.String">Environmental Archaeology Lab Dept. of Philosophical, Historical &amp; Religious Studes</address1>
      <address2 class="java.lang.String">Umeå University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">mattias.sjolander@umu.se</email>
      <firstName class="java.lang.String">Mattias</firstName>
      <lastName class="java.lang.String">Sjölander</lastName>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">http://www.idesam.umu.se/om/personal/?uid=masj0062&amp;guiseId=360086&amp;orgId=4864cb4234d0bf7c77c65d7f78ffca7ecaf285c7&amp;name=Mattias%20Sj%c3%b6lander</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
    <com.sead.database.TblContacts id="12">
      <address1 class="java.lang.String">The Laboratory for Ceramic Research</address1>
      <address2 class="java.lang.String">Lund University</address2>
      <contactId class="java.lang.Integer">NULL</contactId>
      <email class="java.lang.String">anders.lindahl@geol.lu.se</email>
      <firstName class="java.lang.String">KFL</firstName>
      <lastName class="java.lang.String">NULL</lastName>
      <locationId class="com.sead.database.TblLocations" id="1666"/>
      <phoneNumber class="java.lang.String">NULL</phoneNumber>
      <url class="java.lang.Character">http://www.geology.lu.se/research/laboratories-equipment/the-laboratory-for-ceramic-research</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblContacts>
  </TblContacts>
  <TblDatasetContacts length="11076">
    <com.sead.database.TblDatasetContacts id="1">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3024"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="2">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3025"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="3">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3026"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="4">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3027"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="5">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3028"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="6">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3029"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="7">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3030"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="8">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3031"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="9">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3032"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="10">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3033"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="11">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3034"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
    <com.sead.database.TblDatasetContacts id="12">
      <contactId class="com.sead.database.TblContacts" id="12"/>
      <contactTypeId class="TblContactTypes">2.0</contactTypeId>
      <datasetContactId class="java.lang.Integer">NULL</datasetContactId>
      <datasetId class="com.sead.database.TblDatasets" id="3035"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetContacts>
  </TblDatasetContacts>
  <TblDatasetMasters length="1">
    <com.sead.database.TblDatasetMasters id="3">
      <masterName class="java.lang.String">The Laboratory for Ceramic Research (Lund/KFL)</masterName>
      <masterNotes class="java.lang.Character">Data created by staff at the Laboratory for Ceramic Research at Lund University, Sweden.</masterNotes>
      <masterSetId class="java.lang.Integer">NULL</masterSetId>
      <url class="java.lang.Character">http://www.geology.lu.se/research/laboratories-equipment/the-laboratory-for-ceramic-research</url>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetMasters>
  </TblDatasetMasters>
  <TblDatasetSubmissionTypes length="12">
    <com.sead.database.TblDatasetSubmissionTypes id="1" cloneId="1"/>
    <com.sead.database.TblDatasetSubmissionTypes id="2" cloneId="2"/>
    <com.sead.database.TblDatasetSubmissionTypes id="3" cloneId="3"/>
    <com.sead.database.TblDatasetSubmissionTypes id="4" cloneId="4"/>
    <com.sead.database.TblDatasetSubmissionTypes id="5" cloneId="5"/>
    <com.sead.database.TblDatasetSubmissionTypes id="6" cloneId="6"/>
    <com.sead.database.TblDatasetSubmissionTypes id="7" cloneId="7"/>
    <com.sead.database.TblDatasetSubmissionTypes id="8" cloneId="8"/>
    <com.sead.database.TblDatasetSubmissionTypes id="9" cloneId="9"/>
    <com.sead.database.TblDatasetSubmissionTypes id="10" cloneId="10"/>
    <com.sead.database.TblDatasetSubmissionTypes id="11" cloneId="11"/>
    <com.sead.database.TblDatasetSubmissionTypes id="12">
      <description class="java.lang.Character">Who registered the samples at the lab/museum</description>
      <submissionType class="java.lang.String">Samples registered by</submissionType>
      <submissionTypeId class="java.lang.Integer">NULL</submissionTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissionTypes>
  </TblDatasetSubmissionTypes>
  <TblDatasetSubmissions length="34148">
    <com.sead.database.TblDatasetSubmissions id="1">
      <contactId class="com.sead.database.TblContacts" id="10"/>
      <datasetId class="com.sead.database.TblDatasets" id="3024"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="3" cloneId="3"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="2">
      <contactId class="com.sead.database.TblContacts" id="3"/>
      <datasetId class="com.sead.database.TblDatasets" id="3024"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">Registered by: KFL. Registry date missing</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="11" cloneId="11"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="3">
      <contactId class="com.sead.database.TblContacts" id="11"/>
      <datasetId class="com.sead.database.TblDatasets" id="3024"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2017-09-19 09:05:52</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="5" cloneId="5"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="4">
      <contactId class="com.sead.database.TblContacts" id="10"/>
      <datasetId class="com.sead.database.TblDatasets" id="3025"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="3" cloneId="3"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="5">
      <contactId class="com.sead.database.TblContacts" id="3"/>
      <datasetId class="com.sead.database.TblDatasets" id="3025"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">Registered by: KFL. Registry date missing</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="11" cloneId="11"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="6">
      <contactId class="com.sead.database.TblContacts" id="11"/>
      <datasetId class="com.sead.database.TblDatasets" id="3025"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2017-09-19 09:05:52</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="5" cloneId="5"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="7">
      <contactId class="com.sead.database.TblContacts" id="10"/>
      <datasetId class="com.sead.database.TblDatasets" id="3026"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="3" cloneId="3"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="8">
      <contactId class="com.sead.database.TblContacts" id="3"/>
      <datasetId class="com.sead.database.TblDatasets" id="3026"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">Registered by: KFL. Registry date missing</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="11" cloneId="11"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="9">
      <contactId class="com.sead.database.TblContacts" id="11"/>
      <datasetId class="com.sead.database.TblDatasets" id="3026"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2017-09-19 09:05:52</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="5" cloneId="5"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="10">
      <contactId class="com.sead.database.TblContacts" id="10"/>
      <datasetId class="com.sead.database.TblDatasets" id="3027"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="3" cloneId="3"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="11">
      <contactId class="com.sead.database.TblContacts" id="3"/>
      <datasetId class="com.sead.database.TblDatasets" id="3027"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2011-11-09 00:00:00</dateSubmitted>
      <notes class="java.lang.Character">Registered by: KFL. Registry date missing</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="11" cloneId="11"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
    <com.sead.database.TblDatasetSubmissions id="12">
      <contactId class="com.sead.database.TblContacts" id="11"/>
      <datasetId class="com.sead.database.TblDatasets" id="3027"/>
      <datasetSubmissionId class="java.lang.Integer">NULL</datasetSubmissionId>
      <dateSubmitted class="java.sql.Date">2017-09-19 09:05:52</dateSubmitted>
      <notes class="java.lang.Character">NULL</notes>
      <submissionTypeId class="com.sead.database.TblDatasetSubmissionTypes" id="5" cloneId="5"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasetSubmissions>
  </TblDatasetSubmissions>
  <TblDatasets length="11076">
    <com.sead.database.TblDatasets id="3024">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">1@50 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3025">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">2@51 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3026">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">3@52 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3027">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">4@53 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3028">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">5@123 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3029">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">6@2 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3030">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">7@3 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3031">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">8@4 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3032">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">9@5 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3033">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">10@8 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3034">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">11@10 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
    <com.sead.database.TblDatasets id="3035">
      <dataTypeId class="TblDataTypes">18</dataTypeId>
      <datasetId class="java.lang.Integer">NULL</datasetId>
      <datasetName class="java.lang.String">12@127 Petrographic</datasetName>
      <masterSetId class="com.sead.database.TblDatasetMasters" id="3"/>
      <methodId class="com.sead.database.TblMethods" id="158"/>
      <updatedDatasetId class="TblDatasets">NULL</updatedDatasetId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDatasets>
  </TblDatasets>
  <TblDimensions length="41">
    <com.sead.database.TblDimensions id="1" cloneId="1"/>
    <com.sead.database.TblDimensions id="2" cloneId="2"/>
    <com.sead.database.TblDimensions id="3" cloneId="3"/>
    <com.sead.database.TblDimensions id="4" cloneId="4"/>
    <com.sead.database.TblDimensions id="5" cloneId="5"/>
    <com.sead.database.TblDimensions id="6" cloneId="6"/>
    <com.sead.database.TblDimensions id="7" cloneId="7"/>
    <com.sead.database.TblDimensions id="9" cloneId="9"/>
    <com.sead.database.TblDimensions id="10" cloneId="10"/>
    <com.sead.database.TblDimensions id="11" cloneId="11"/>
    <com.sead.database.TblDimensions id="12" cloneId="12"/>
    <com.sead.database.TblDimensions id="13" cloneId="13"/>
    <com.sead.database.TblDimensions id="14" cloneId="14"/>
    <com.sead.database.TblDimensions id="15" cloneId="15"/>
    <com.sead.database.TblDimensions id="16" cloneId="16"/>
    <com.sead.database.TblDimensions id="17" cloneId="17"/>
    <com.sead.database.TblDimensions id="18" cloneId="18"/>
    <com.sead.database.TblDimensions id="19" cloneId="19"/>
    <com.sead.database.TblDimensions id="20" cloneId="20"/>
    <com.sead.database.TblDimensions id="21" cloneId="21"/>
    <com.sead.database.TblDimensions id="22" cloneId="22"/>
    <com.sead.database.TblDimensions id="23" cloneId="23"/>
    <com.sead.database.TblDimensions id="24" cloneId="24"/>
    <com.sead.database.TblDimensions id="25" cloneId="25"/>
    <com.sead.database.TblDimensions id="26" cloneId="26"/>
    <com.sead.database.TblDimensions id="27" cloneId="27"/>
    <com.sead.database.TblDimensions id="28" cloneId="28"/>
    <com.sead.database.TblDimensions id="29" cloneId="29"/>
    <com.sead.database.TblDimensions id="30" cloneId="30"/>
    <com.sead.database.TblDimensions id="31" cloneId="31"/>
    <com.sead.database.TblDimensions id="32" cloneId="32"/>
    <com.sead.database.TblDimensions id="33" cloneId="33"/>
    <com.sead.database.TblDimensions id="34" cloneId="34"/>
    <com.sead.database.TblDimensions id="35">
      <dimensionAbbrev class="java.lang.String">Base minimum</dimensionAbbrev>
      <dimensionDescription class="java.lang.Character">Vessel base diameter, minimum size measurement</dimensionDescription>
      <dimensionId class="java.lang.Integer">NULL</dimensionId>
      <dimensionName class="java.lang.String">Base diameter(cm) minimum</dimensionName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblDimensions>
  </TblDimensions>
  <TblFeatureTypes length="67">
    <com.sead.database.TblFeatureTypes id="1" cloneId="1"/>
    <com.sead.database.TblFeatureTypes id="2" cloneId="2"/>
    <com.sead.database.TblFeatureTypes id="4" cloneId="4"/>
    <com.sead.database.TblFeatureTypes id="5" cloneId="5"/>
    <com.sead.database.TblFeatureTypes id="6" cloneId="6"/>
    <com.sead.database.TblFeatureTypes id="7" cloneId="7"/>
    <com.sead.database.TblFeatureTypes id="8" cloneId="8"/>
    <com.sead.database.TblFeatureTypes id="9" cloneId="9"/>
    <com.sead.database.TblFeatureTypes id="10" cloneId="10"/>
    <com.sead.database.TblFeatureTypes id="11" cloneId="11"/>
    <com.sead.database.TblFeatureTypes id="12" cloneId="12"/>
    <com.sead.database.TblFeatureTypes id="13" cloneId="13"/>
    <com.sead.database.TblFeatureTypes id="14" cloneId="14"/>
    <com.sead.database.TblFeatureTypes id="15" cloneId="15"/>
    <com.sead.database.TblFeatureTypes id="16" cloneId="16"/>
    <com.sead.database.TblFeatureTypes id="17" cloneId="17"/>
    <com.sead.database.TblFeatureTypes id="18" cloneId="18"/>
    <com.sead.database.TblFeatureTypes id="19" cloneId="19"/>
    <com.sead.database.TblFeatureTypes id="20" cloneId="20"/>
    <com.sead.database.TblFeatureTypes id="21" cloneId="21"/>
    <com.sead.database.TblFeatureTypes id="22" cloneId="22"/>
    <com.sead.database.TblFeatureTypes id="23" cloneId="23"/>
    <com.sead.database.TblFeatureTypes id="24" cloneId="24"/>
    <com.sead.database.TblFeatureTypes id="25" cloneId="25"/>
    <com.sead.database.TblFeatureTypes id="26" cloneId="26"/>
    <com.sead.database.TblFeatureTypes id="27" cloneId="27"/>
    <com.sead.database.TblFeatureTypes id="28" cloneId="28"/>
    <com.sead.database.TblFeatureTypes id="29" cloneId="29"/>
    <com.sead.database.TblFeatureTypes id="30" cloneId="30"/>
    <com.sead.database.TblFeatureTypes id="31" cloneId="31"/>
    <com.sead.database.TblFeatureTypes id="32" cloneId="32"/>
    <com.sead.database.TblFeatureTypes id="33" cloneId="33"/>
    <com.sead.database.TblFeatureTypes id="34" cloneId="34"/>
    <com.sead.database.TblFeatureTypes id="35" cloneId="35"/>
    <com.sead.database.TblFeatureTypes id="36" cloneId="36"/>
    <com.sead.database.TblFeatureTypes id="37" cloneId="37"/>
    <com.sead.database.TblFeatureTypes id="38" cloneId="38"/>
    <com.sead.database.TblFeatureTypes id="39" cloneId="39"/>
    <com.sead.database.TblFeatureTypes id="40" cloneId="40"/>
    <com.sead.database.TblFeatureTypes id="41" cloneId="41"/>
    <com.sead.database.TblFeatureTypes id="42" cloneId="42"/>
    <com.sead.database.TblFeatureTypes id="43">
      <featureTypeDescription class="java.lang.Character">A site previously inhabited where worked objects, raw material, refuse and/or remains from buildings are left. </featureTypeDescription>
      <featureTypeId class="java.lang.Integer">NULL</featureTypeId>
      <featureTypeName class="java.lang.String">Settlement site</featureTypeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatureTypes>
  </TblFeatureTypes>
  <TblFeatures length="935">
    <com.sead.database.TblFeatures id="1832">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">Feature</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1833">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">1854</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1834">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">3748</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1835">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">19049</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1836">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">gånggrift</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="6" cloneId="6"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1837">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">Feature</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="56"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1838">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">gånggrift</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="6" cloneId="6"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1839">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">6021</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1840">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">4412c</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1841">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">5033</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1842">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">4251</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
    <com.sead.database.TblFeatures id="1843">
      <featureDescription class="java.lang.Character">NULL</featureDescription>
      <featureId class="java.lang.Integer">NULL</featureId>
      <featureName class="java.lang.String">6101</featureName>
      <featureTypeId class="com.sead.database.TblFeatureTypes" id="43"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblFeatures>
  </TblFeatures>
  <TblLocations length="377">
    <com.sead.database.TblLocations id="1666">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Lund</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1667">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Aldenhovener Platte</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1668">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Attica</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1669">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Bagamoyo</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1670">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Bornholm</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1671">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Buhera</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1672">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Buskerud</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1673">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Chachapoyas</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1674">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Varsinais-Suomi</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1675">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Etruria</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1676">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Fyn</locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
    <com.sead.database.TblLocations id="1677">
      <defaultLatDd class="java.lang.Double">NULL</defaultLatDd>
      <defaultLongDd class="java.lang.Double">NULL</defaultLongDd>
      <locationId class="java.lang.Integer">NULL</locationId>
      <locationName class="java.lang.String">Odense </locationName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblLocations>
  </TblLocations>
  <TblMethods length="3">
    <com.sead.database.TblMethods id="156">
      <description class="java.lang.Character">A piece of the ceramic sherd is cut off, fastened to a microscope slide, ground down to 0,03 mm, and polished. The tempering grains and clay is then studied under a microscope to determine clay content, fraction size, sorting and tempering content. These are all determined ocularly. The max grain size and total percent tempering is determined by marking the tempering grains in a photograph projected from the camera connected to the microscope and calculating in the program. </description>
      <methodAbbrevOrAltName class="java.lang.String">Petrographic</methodAbbrevOrAltName>
      <methodId class="java.lang.Integer">NULL</methodId>
      <methodName class="java.lang.String">Petrographic microscopy</methodName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblMethods>
    <com.sead.database.TblMethods id="157">
      <description class="java.lang.Character">A raw clay sample and a sample of the ceramic sherd, or archaeological sample, is heated in a lab oven in 100 °C intervals, for a total of 30 minutes, up to 1000 °C. The colour change during and after heating is noted and compared to the Munsell Colour Chart to determine the firing temperature and firing atmosphere of the archaeological sample. The samples are then once more heated above 1000 °C, in 50 °C intervals, until they reach their melting point. The assumption for the thermal analysis is that the colour change of the ceramic sample doesnt occur until after the original firing temperature has been reached. </description>
      <methodAbbrevOrAltName class="java.lang.String">Thermal</methodAbbrevOrAltName>
      <methodId class="java.lang.Integer">NULL</methodId>
      <methodName class="java.lang.String">Thermal analysis</methodName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblMethods>
    <com.sead.database.TblMethods id="158">
      <description class="java.lang.Character">Collection of ceramic samples by unspecified means</description>
      <methodAbbrevOrAltName class="java.lang.String">Ceramics</methodAbbrevOrAltName>
      <methodId class="java.lang.Integer">NULL</methodId>
      <methodName class="java.lang.String">Ceramics collection</methodName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblMethods>
  </TblMethods>
  <TblPhysicalSampleFeatures length="3280">
    <com.sead.database.TblPhysicalSampleFeatures id="4834">
      <featureId class="com.sead.database.TblFeatures" id="1832"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35559"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4835">
      <featureId class="com.sead.database.TblFeatures" id="1832"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35560"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4836">
      <featureId class="com.sead.database.TblFeatures" id="1832"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35561"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4837">
      <featureId class="com.sead.database.TblFeatures" id="1832"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35562"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4838">
      <featureId class="com.sead.database.TblFeatures" id="1833"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35122"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4839">
      <featureId class="com.sead.database.TblFeatures" id="1833"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35123"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4840">
      <featureId class="com.sead.database.TblFeatures" id="1833"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35124"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4841">
      <featureId class="com.sead.database.TblFeatures" id="1834"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35125"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4842">
      <featureId class="com.sead.database.TblFeatures" id="1834"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35126"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4843">
      <featureId class="com.sead.database.TblFeatures" id="1834"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35127"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4844">
      <featureId class="com.sead.database.TblFeatures" id="1834"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35128"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
    <com.sead.database.TblPhysicalSampleFeatures id="4845">
      <featureId class="com.sead.database.TblFeatures" id="1835"/>
      <physicalSampleFeatureId class="java.lang.Integer">NULL</physicalSampleFeatureId>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="35129"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSampleFeatures>
  </TblPhysicalSampleFeatures>
  <TblPhysicalSamples length="3499">
    <com.sead.database.TblPhysicalSamples id="32800">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2224"/>
      <sampleName class="java.lang.String">1@50@Lampa</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32801">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2225"/>
      <sampleName class="java.lang.String">2@51@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32802">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2226"/>
      <sampleName class="java.lang.String">3@52@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32803">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2227"/>
      <sampleName class="java.lang.String">4@53@Lampa</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32804">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2228"/>
      <sampleName class="java.lang.String">5@123@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32805">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2229"/>
      <sampleName class="java.lang.String">6@2@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32806">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2230"/>
      <sampleName class="java.lang.String">7@3@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32807">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2231"/>
      <sampleName class="java.lang.String">8@4@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32808">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2232"/>
      <sampleName class="java.lang.String">9@5@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32809">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2233"/>
      <sampleName class="java.lang.String">10@8@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32810">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2234"/>
      <sampleName class="java.lang.String">11@10@Lampa</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
    <com.sead.database.TblPhysicalSamples id="32811">
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="11"/>
      <dateSampled class="java.lang.String">NULL</dateSampled>
      <physicalSampleId class="java.lang.Integer">NULL</physicalSampleId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2235"/>
      <sampleName class="java.lang.String">12@127@Kärl</sampleName>
      <sampleTypeId class="com.sead.database.TblSampleTypes" id="15"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblPhysicalSamples>
  </TblPhysicalSamples>
  <TblRelativeAges length="137">
    <com.sead.database.TblRelativeAges id="389">
      <abbreviation class="java.lang.String">Quaternary</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">2700000.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">NULL</calAgeYounger>
      <description class="java.lang.Character">Geologic period.</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Quaternary </relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="390">
      <abbreviation class="java.lang.String">Mesolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">13000.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">5150.0</calAgeYounger>
      <description class="java.lang.Character">Mesolithic period as defined in BugsCEP.</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Mesolithic S Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="391">
      <abbreviation class="java.lang.String">Mesolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">13000.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">5150.0</calAgeYounger>
      <description class="java.lang.Character">Mesolithic period as defined in BugsCEP.</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Mesolithic N Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="392">
      <abbreviation class="java.lang.String">Neolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">5150.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">3800.0</calAgeYounger>
      <description class="java.lang.Character">Neolithic period as defined in BugsCEP.</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Neolithic S Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="393">
      <abbreviation class="java.lang.String">Neolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">5150.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">3800.0</calAgeYounger>
      <description class="java.lang.Character">Neolithic period as defined in BugsCEP.</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Neolithic N Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="394">
      <abbreviation class="java.lang.String">Neolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">6000.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">5300.0</calAgeYounger>
      <description class="java.lang.Character">Early neolithic for S Scandinavia as defined by T.Douglas Price (2016).</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Early neolithic S Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="395">
      <abbreviation class="java.lang.String">Neolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">5300.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">4400.0</calAgeYounger>
      <description class="java.lang.Character">Middle neolithic for S Scandinavia as defined by T.Douglas Price (2016).</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Middle neolithic S Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="396">
      <abbreviation class="java.lang.String">Neolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">4400.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">3800.0</calAgeYounger>
      <description class="java.lang.Character">Late neolithic for S Scandinavia as defined by T.Douglas Price (2016).</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Late neolithic S Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="397">
      <abbreviation class="java.lang.String">Epineolithic</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">4000.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">2800.0</calAgeYounger>
      <description class="java.lang.Character">Epineolithic for northern Sweden as defined by E. Baudou (1993).</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Epineolithic N Sweden </relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="398">
      <abbreviation class="java.lang.String">Bronze Age</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">3800.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">2500.0</calAgeYounger>
      <description class="java.lang.Character">Bronze age period as defined in BugsCEP.</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Bronze Age Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="399">
      <abbreviation class="java.lang.String">Bronze Age</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">3800.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">3100.0</calAgeYounger>
      <description class="java.lang.Character">Early Bronze Age as defined by T. Douglas Price (2016).</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Early Bronze Age Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
    <com.sead.database.TblRelativeAges id="400">
      <abbreviation class="java.lang.String">Bronze Age</abbreviation>
      <c14AgeOlder class="java.lang.Double">NULL</c14AgeOlder>
      <c14AgeYounger class="java.lang.Double">NULL</c14AgeYounger>
      <calAgeOlder class="java.lang.Double">3100.0</calAgeOlder>
      <calAgeYounger class="java.lang.Double">2500.0</calAgeYounger>
      <description class="java.lang.Character">Late Bronze Age as defined by T. Douglas Price (2016).</description>
      <notes class="java.lang.Character">NULL</notes>
      <relativeAgeId class="java.lang.Integer">NULL</relativeAgeId>
      <relativeAgeName class="java.lang.String">Late Bronze Age Scandinavia</relativeAgeName>
      <relativeAgeTypeId class="TblRelativeAgeTypes">1</relativeAgeTypeId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeAges>
  </TblRelativeAges>
  <TblRelativeDates length="10824">
    <com.sead.database.TblRelativeDates id="8207">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99042"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8208">
      <notes class="java.lang.Character">Original date description: Medeltid Culture: Västeuropeisk medeltid</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99907"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8209">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="103394"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8210">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="106793"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8211">
      <notes class="java.lang.Character">Original date description: Medeltid Culture: Västeuropeisk medeltid</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99908"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8212">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="103395"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8213">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="106794"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8214">
      <notes class="java.lang.Character">Original date description: Medeltid Culture: Västeuropeisk medeltid</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99909"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8215">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="103396"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8216">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="106795"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8217">
      <notes class="java.lang.Character">Original date description: Medeltid Culture: Västeuropeisk medeltid</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="99910"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
    <com.sead.database.TblRelativeDates id="8218">
      <notes class="java.lang.Character">Original date description: Mesolitikum-TN I Culture: Ertebölle</notes>
      <analysisEntityId class="com.sead.database.TblAnalysisEntities" id="103397"/>
      <relativeAgeId class="com.sead.database.TblRelativeAges" id="498"/>
      <relativeDateId class="java.lang.Integer">NULL</relativeDateId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblRelativeDates>
  </TblRelativeDates>
  <TblSampleAltRefs length="1959">
    <com.sead.database.TblSampleAltRefs id="9788">
      <altRef class="java.lang.String">shm 33097 eller 31931</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32887"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9789">
      <altRef class="java.lang.String">shm26782?</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32922"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9790">
      <altRef class="java.lang.String">shm26782?</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32923"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9791">
      <altRef class="java.lang.String">mhm 4731, 4730 eller 4555-4559?</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32945"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9792">
      <altRef class="java.lang.String">SHM 16025 &amp; 33762 </altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32991"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9793">
      <altRef class="java.lang.String">lösfynd</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="10"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32991"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9794">
      <altRef class="java.lang.String">SHM 16025 &amp; 33762 </altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32992"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9795">
      <altRef class="java.lang.String">lösfynd</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="10"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32992"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9796">
      <altRef class="java.lang.String">SHM 16025 &amp; 33762 </altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32993"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9797">
      <altRef class="java.lang.String">A6</altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="10"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32993"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9798">
      <altRef class="java.lang.String">SHM 16025 &amp; 33762 </altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32994"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
    <com.sead.database.TblSampleAltRefs id="9799">
      <altRef class="java.lang.String">SHM 16025 &amp; 33762 </altRef>
      <altRefTypeId class="com.sead.database.TblAltRefTypes" id="1" cloneId="1"/>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32995"/>
      <sampleAltRefId class="java.lang.Integer">NULL</sampleAltRefId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleAltRefs>
  </TblSampleAltRefs>
  <TblSampleDimensions length="1740">
    <com.sead.database.TblSampleDimensions id="749">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">12.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32800"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="750">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">19.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32801"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="751">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">14.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32802"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="752">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">18.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32803"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="753">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">15.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32804"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="754">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">15.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32805"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="755">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">17.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32806"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="756">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">13.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32807"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="757">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">17.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32808"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="758">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">15.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32809"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="759">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">18.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32810"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
    <com.sead.database.TblSampleDimensions id="760">
      <dimensionId class="com.sead.database.TblDimensions" id="34" cloneId="34"/>
      <dimensionValue class="java.lang.Double">11.0</dimensionValue>
      <physicalSampleId class="com.sead.database.TblPhysicalSamples" id="32811"/>
      <sampleDimensionId class="java.lang.Integer">NULL</sampleDimensionId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleDimensions>
  </TblSampleDimensions>
  <TblSampleGroupDescriptionTypeSamplingContexts length="41">
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="16">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="4"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="17">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="5"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="18">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="6"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="19">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="7"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="20">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="8"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="21">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="8"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="22">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="9"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="23">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="10"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="24">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="25">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="12"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="26">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="13"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
    <com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts id="27">
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="14"/>
      <sampleGroupDescriptionTypeSamplingContextId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeSamplingContextId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypeSamplingContexts>
  </TblSampleGroupDescriptionTypeSamplingContexts>
  <TblSampleGroupDescriptionTypes length="29">
    <com.sead.database.TblSampleGroupDescriptionTypes id="1" cloneId="1"/>
    <com.sead.database.TblSampleGroupDescriptionTypes id="2" cloneId="2"/>
    <com.sead.database.TblSampleGroupDescriptionTypes id="3" cloneId="3"/>
    <com.sead.database.TblSampleGroupDescriptionTypes id="4">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">A tube, nozzle or pipe used for blowing air into a furnace or hearth</typeDescription>
      <typeName class="java.lang.String">Tuyere</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="5">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Fireproof container used for smelting metal or glass.</typeDescription>
      <typeName class="java.lang.String">Crucible</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="6">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Any kind of figurine made out various materials</typeDescription>
      <typeName class="java.lang.String">Figurine</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="7">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Any type of of mold used in in the production of metal and glass work</typeDescription>
      <typeName class="java.lang.String">Mould</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="8">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Any form of sample of soil used in various types of analyses</typeDescription>
      <typeName class="java.lang.String">Soil sample</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="9">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Earthenware tiles with any form of glazing </typeDescription>
      <typeName class="java.lang.String">Glazed tile</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="10">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">A specific type of tiled kiln/oven, common to areas such as Germany, Sweden and Scandinavia. The kiln is decorated with glazed tiles, ranging from pure white to decorated ones.  </typeDescription>
      <typeName class="java.lang.String">Tiled kiln/Kachelofen</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="11">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Any type of container for the storage or utilisation of products, especially food or drink.</typeDescription>
      <typeName class="java.lang.String">Vessel</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
    <com.sead.database.TblSampleGroupDescriptionTypes id="12">
      <sampleGroupDescriptionTypeId class="java.lang.Integer">NULL</sampleGroupDescriptionTypeId>
      <typeDescription class="java.lang.String">Denotes uncertainty in the identification</typeDescription>
      <typeName class="java.lang.String">Vessel?</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptionTypes>
  </TblSampleGroupDescriptionTypes>
  <TblSampleGroupDescriptions length="3867">
    <com.sead.database.TblSampleGroupDescriptions id="1">
      <sampleGroupDescription class="java.lang.String">Lampa: Lampa</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="14"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2224"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="2">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2225"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="3">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2226"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="4">
      <sampleGroupDescription class="java.lang.String">Lampa: Lampa</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="14"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2227"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="5">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2228"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="6">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2229"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="7">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2230"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="8">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2231"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="9">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2232"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="10">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2233"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="11">
      <sampleGroupDescription class="java.lang.String">Lampa: Lampa</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="14"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2234"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
    <com.sead.database.TblSampleGroupDescriptions id="12">
      <sampleGroupDescription class="java.lang.String">Kärl: Undefined</sampleGroupDescription>
      <sampleGroupDescriptionId class="java.lang.Integer">NULL</sampleGroupDescriptionId>
      <sampleGroupDescriptionTypeId class="com.sead.database.TblSampleGroupDescriptionTypes" id="11"/>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2235"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDescriptions>
  </TblSampleGroupDescriptions>
  <TblSampleGroupDimensions length="132">
    <com.sead.database.TblSampleGroupDimensions id="3">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">18.0</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2716"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="4">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">11.5</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2818"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="5">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">11.5</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2819"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="6">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">15.5</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2820"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="7">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">16.2</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2821"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="8">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">14.3</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2822"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="9">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">9.5</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="10">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">11.7</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2824"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="11">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">11.7</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2825"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="12">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">11.2</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2826"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="13">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">16.0</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2827"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
    <com.sead.database.TblSampleGroupDimensions id="14">
      <dimensionId class="com.sead.database.TblDimensions" id="31" cloneId="31"/>
      <dimensionValue class="java.lang.Double">11.4</dimensionValue>
      <sampleGroupDimensionId class="java.lang.Integer">NULL</sampleGroupDimensionId>
      <sampleGroupId class="com.sead.database.TblSampleGroups" id="2828"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroupDimensions>
  </TblSampleGroupDimensions>
  <TblSampleGroups length="3499">
    <com.sead.database.TblSampleGroups id="2224">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 1@Lampa</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1676"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2225">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 2@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1970"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2226">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 3@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1970"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2227">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 4@Lampa</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1970"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2228">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 5@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1906"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2229">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 6@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2230">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 7@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2231">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 8@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2232">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 9@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2233">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 10@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2234">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 11@Lampa</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1823"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
    <com.sead.database.TblSampleGroups id="2235">
      <sampleGroupDescription class="java.lang.Character">NULL</sampleGroupDescription>
      <sampleGroupId class="java.lang.Integer">NULL</sampleGroupId>
      <sampleGroupName class="java.lang.String">KFL 12@Kärl</sampleGroupName>
      <siteId class="com.sead.database.TblSites" id="1725"/>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleGroups>
  </TblSampleGroups>
  <TblSampleTypes length="15">
    <com.sead.database.TblSampleTypes id="1" cloneId="1"/>
    <com.sead.database.TblSampleTypes id="2" cloneId="2"/>
    <com.sead.database.TblSampleTypes id="3" cloneId="3"/>
    <com.sead.database.TblSampleTypes id="4" cloneId="4"/>
    <com.sead.database.TblSampleTypes id="5" cloneId="5"/>
    <com.sead.database.TblSampleTypes id="6" cloneId="6"/>
    <com.sead.database.TblSampleTypes id="7" cloneId="7"/>
    <com.sead.database.TblSampleTypes id="8" cloneId="8"/>
    <com.sead.database.TblSampleTypes id="9" cloneId="9"/>
    <com.sead.database.TblSampleTypes id="10" cloneId="10"/>
    <com.sead.database.TblSampleTypes id="11" cloneId="11"/>
    <com.sead.database.TblSampleTypes id="12" cloneId="12"/>
    <com.sead.database.TblSampleTypes id="13" cloneId="13"/>
    <com.sead.database.TblSampleTypes id="14" cloneId="14"/>
    <com.sead.database.TblSampleTypes id="15">
      <description class="java.lang.Character">A sherd originating from any type of ceramic object</description>
      <sampleTypeId class="java.lang.Integer">NULL</sampleTypeId>
      <typeName class="java.lang.String">Ceramic sherd</typeName>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSampleTypes>
  </TblSampleTypes>
  <TblSiteLocations length="1219">
    <com.sead.database.TblSiteLocations id="4567">
      <siteId class="com.sead.database.TblSites" id="1567"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4568">
      <locationId class="com.sead.database.TblLocations" id="1724"/>
      <siteId class="com.sead.database.TblSites" id="1567"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4569">
      <locationId class="com.sead.database.TblLocations" id="1839"/>
      <siteId class="com.sead.database.TblSites" id="1567"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4570">
      <siteId class="com.sead.database.TblSites" id="1568"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4571">
      <siteId class="com.sead.database.TblSites" id="1568"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4572">
      <siteId class="com.sead.database.TblSites" id="1568"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4573">
      <siteId class="com.sead.database.TblSites" id="1569"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4574">
      <locationId class="com.sead.database.TblLocations" id="1725"/>
      <siteId class="com.sead.database.TblSites" id="1569"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4575">
      <locationId class="com.sead.database.TblLocations" id="2026"/>
      <siteId class="com.sead.database.TblSites" id="1569"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4576">
      <siteId class="com.sead.database.TblSites" id="1570"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4577">
      <locationId class="com.sead.database.TblLocations" id="1725"/>
      <siteId class="com.sead.database.TblSites" id="1570"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
    <com.sead.database.TblSiteLocations id="4578">
      <locationId class="com.sead.database.TblLocations" id="2026"/>
      <siteId class="com.sead.database.TblSites" id="1570"/>
      <siteLocationId class="java.lang.Integer">NULL</siteLocationId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteLocations>
  </TblSiteLocations>
  <TblSiteReferences length="451">
    <com.sead.database.TblSiteReferences id="95">
      <siteId class="com.sead.database.TblSites" id="1567"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="96">
      <siteId class="com.sead.database.TblSites" id="1567"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="97">
      <siteId class="com.sead.database.TblSites" id="1568"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="98">
      <siteId class="com.sead.database.TblSites" id="1569"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="99">
      <siteId class="com.sead.database.TblSites" id="1569"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="100">
      <siteId class="com.sead.database.TblSites" id="1570"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="101">
      <siteId class="com.sead.database.TblSites" id="1571"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="102">
      <siteId class="com.sead.database.TblSites" id="1573"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="103">
      <siteId class="com.sead.database.TblSites" id="1573"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="104">
      <siteId class="com.sead.database.TblSites" id="1574"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="105">
      <siteId class="com.sead.database.TblSites" id="1575"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
    <com.sead.database.TblSiteReferences id="106">
      <siteId class="com.sead.database.TblSites" id="1575"/>
      <siteReferenceId class="java.lang.Integer">NULL</siteReferenceId>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSiteReferences>
  </TblSiteReferences>
  <TblSites length="421">
    <com.sead.database.TblSites id="1567">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">56.4894623637559</latitudeDd>
      <longitudeDd class="java.lang.Double">16.5763567902096</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Hulterstad 114?</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Alby</siteName>
      <siteLocationAccuracy class="java.lang.string">Poor</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1568">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">55.5603451969822</latitudeDd>
      <longitudeDd class="java.lang.Double">12.9450954078966</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Bunkeflo 24</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Almhov- delomr. 1</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1569">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">58.295565830262</latitudeDd>
      <longitudeDd class="java.lang.Double">14.6483695135992</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Västra Tollstad 12</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Alvastra</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1570">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">58.295565830262</latitudeDd>
      <longitudeDd class="java.lang.Double">14.6483695135992</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Västra Tollstad 12</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Alvastra Megalit</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1571">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">57.7142363439112</latitudeDd>
      <longitudeDd class="java.lang.Double">11.7782901544265</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Torslanda 110</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Amhult</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1572">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">58.2060432093195</latitudeDd>
      <longitudeDd class="java.lang.Double">11.91025474837</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Ljung 44?</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Anfasterö</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1573">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">55.826834424693</latitudeDd>
      <longitudeDd class="java.lang.Double">13.0062972237262</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Annelöv 13</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Annelöv</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1574">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">57.2277522941254</latitudeDd>
      <longitudeDd class="java.lang.Double">18.3716316340893</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Hemse 51?</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Annexhemmanet</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1575">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">59.594589</latitudeDd>
      <longitudeDd class="java.lang.Double">17.462282</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">Övergran 260</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Apalle</siteName>
      <siteLocationAccuracy class="java.lang.string">Ancient monument</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1576">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">37.527456</latitudeDd>
      <longitudeDd class="java.lang.Double">22.874454</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">NULL</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Asine</siteName>
      <siteLocationAccuracy class="java.lang.string">Approximate</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1577">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">-6.232948</latitudeDd>
      <longitudeDd class="java.lang.Double">-77.860742</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">NULL</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Atuen</siteName>
      <siteLocationAccuracy class="java.lang.string">Poor</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
    <com.sead.database.TblSites id="1578">
      <altitude class="java.lang.Double">NULL</altitude>
      <latitudeDd class="java.lang.Double">56.2078447355679</latitudeDd>
      <longitudeDd class="java.lang.Double">15.6774392049078</longitudeDd>
      <nationalSiteIdentifier class="java.lang.String">NULL</nationalSiteIdentifier>
      <siteDescription class="java.lang.Character">NULL</siteDescription>
      <siteId class="java.lang.Integer">NULL</siteId>
      <siteName class="java.lang.String">Augerum</siteName>
      <siteLocationAccuracy class="java.lang.string">Approximate</siteLocationAccuracy>
      <cloneId class="java.util.Integer">NULL</cloneId>
      <dateUpdated class="java.util.Date"></dateUpdated>
    </com.sead.database.TblSites>
  </TblSites>
</sead-data-upload>
'