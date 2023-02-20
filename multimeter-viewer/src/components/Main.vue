<template>
  <div id="wrapper"> 
    <div class="d-flex tableBar rounded">
      <v-icon slot="prependIcon" color="black" class="searchIcon"
        >mdi-magnify</v-icon
      >
      <div id="tableBarText">
        <h3>Database Select:</h3>
      </div>

      <v-select
        solo
        :items="dbSelectItems"
        dense
        hide-details
        full-width
        v-model="dbSelect"
      >
      </v-select>
      <v-btn
        color="secondary"
        class="ml-2 white--text"
        width="5px"
        @click="getCollections()"
      >
        <v-icon dark>mdi-refresh</v-icon>
      </v-btn>
      <div id="tableBarText">
        <h3>Items: {{dbCount}}</h3>
      </div>
      <v-btn
        v-if="isPaused == false"
        color="#ffb242"
        class="ml-2 white--text"
        width="125px"
        @click="isPaused = !isPaused; getData();"
      >
        <v-icon left dark>mdi-pause</v-icon>Pause
      </v-btn>
      <v-btn
        v-if="isPaused == true"
        color="success"
        class="ml-2 white--text"
        width="125px"
        @click="isPaused = !isPaused; getData();"
      >
        <v-icon left dark>mdi-play</v-icon>Start
      </v-btn>
      <v-btn
        color="success"
        class="ml-2 white--text"
        width="175px"
        @click="downloadCSV()"
      >
        <v-icon left dark>mdi-content-save</v-icon>Download
      </v-btn>
    </div>
    <div id="chartDiv">
       <LineChartGenerator
        :chart-options="chartOptions"
        :chart-data="chartData"
        :chart-id="chartId"
        :dataset-id-key="datasetIdKey"
        :plugins="plugins"
        :css-classes="cssClasses"
        :styles="styles"
        :width="width"
        :height="height"
      />
    </div>
    <div id="dataTable">
      <v-data-table
        :headers="headers"
        :items="data"
        :sort-by.sync="sortBy"
        :sort-desc.sync="sortDesc"
        :items-per-page="-1"
        fixed-header
        height="550px"
      >
      </v-data-table>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { Line as LineChartGenerator } from 'vue-chartjs/legacy'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  LineElement,
  LinearScale,
  CategoryScale,
  PointElement
} from 'chart.js'
ChartJS.register(Title, Tooltip, Legend, LineElement, LinearScale, CategoryScale, PointElement)

export default {
  name: "test",
  props: {
    chartId: {
      type: String,
      default: 'line-chart'
    },
    datasetIdKey: {
      type: String,
      default: 'label'
    },
    width: {
      type: Number,
      default: 400
    },
    height: {
      type: Number,
      default: 400
    },
    cssClasses: {
      default: '',
      type: String
    },
    styles: {
      type: Object,
      default: () => {}
    },
    plugins: {
      type: Array,
      default: () => []
    }
  },
  components: { LineChartGenerator },
  data() {
    return {
      mongoConnection: "http://192.168.238.17:3001",
      dbSelectItems: [],
      dbSelect: "",
      dbCount: 0,
      data: [],
      isPaused: false,
      id: [],
      headers: [
        { text: "Date",  value: "Date",  align: "start"},
        { text: "",  value: "",  align: "start"},
        { text: "",  value: "",  align: "start"},
        { text: "",  value: "",  align: "start"},
        { text: "",  value: "",  align: "start"}
      ],
      sortBy: "Date",
      sortDesc: true,
      chartData: {
        labels: [],
        datasets: [
          {
            label: '',
            backgroundColor: '#80ccff',
            data: [],
            borderColor: "#80ccff"
          },
          {
            label: '',
            backgroundColor: '#ff8080',
            data: [],
            borderColor: "#ff8080"
          },
          {
            label: '',
            backgroundColor: '#80ff80',
            data: [],
            borderColor: "#80ff80"
          },
          {
            label: '',
            backgroundColor: '#ffff80',
            data: [],
            borderColor: "#ffff80"
          }
        ]
      },
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        tooltips: {
          mode: 'index'
        },
        hover: {
          mode: 'index',
        }
      },
      bg: ["#80ccff", "#ff8080", "#80ff80", "#ffff80"]
    };
  },
  methods:{
    getCollections()
    {
      axios.get(this.mongoConnection + '/getCollections').then((response) => {
        this.dbSelectItems = response.data;
        if(this.dbSelect == "")
          this.dbSelect = this.dbSelectItems[0];
      });
    },
    getCount()
    {
      axios.get(this.mongoConnection + '/collection/' + this.dbSelect+"/count").then((response) => {
        this.dbCount = response.data;
      });
    },
    getData()
    {
      axios.get(this.mongoConnection + '/collection/' + this.dbSelect).then((response) => {
        this.getCount();
        for(var i = response.data.length - 1; i > response.data.length - 100; i--)
        {
          var item = response.data[i];
          var today = new Date(item.Date);
          var dd = String(today.getDate()).padStart(2, "0");
          var mm = String(today.getMonth() + 1).padStart(2, "0");
          var yyyy = today.getFullYear();
          var time =
            ("0" + today.getHours()).slice(-2) +
            ":" +
            ("0" + today.getMinutes()).slice(-2) +
            ":" +
            ("0" + today.getSeconds()).slice(-2);
          today = yyyy + "-" + mm + "-" + dd + " " + time;
          var entry = {
            id: item._id,
            Date: today,
          };
          item.Values.forEach((item, index) => {
            if(item != null)
            {
              var isInHeader = false;
              this.headers.forEach((headers) => {
                if(headers.text == item.Name)
                {
                  isInHeader = true;
                }
              })
              if(!isInHeader)
              {
                this.headers[index + 1].text = item.Name;
                this.headers[index + 1].value = item.Name;
                this.chartData.datasets[index].label = item.Name;
              }
              entry[item.Name] = item.Value;
            }
          });
          
          if(!this.id.includes(item._id))
          {
            this.data.push(entry);
            this.id.push(item._id);
            this.chartData.labels.push(today)
            if(this.chartData.labels.length > 30)
            {
              this.data.shift();
              this.chartData.labels.shift();
              this.chartData.datasets[0].data.shift();
              this.chartData.datasets[1].data.shift();
              this.chartData.datasets[2].data.shift();
              this.chartData.datasets[3].data.shift();
            }
            item.Values.forEach((item, index) => {
              if(item != null)
              {
                this.chartData.datasets[index].data.push(item.Value)
              }
              else
              {
                this.chartData.datasets[index].data.push("Null")
              }
            });
          }
        }
      });
      
      setTimeout(() => {
        
        if(!this.isPaused)
        {
          this.getData();
        }
      }, 1000);
    },
    downloadCSV()
    {
      // this.isPaused = true;
      axios.get(this.mongoConnection + '/collection/' + this.dbSelect+"/all").then((response) => {
        var headers = [
          { text: "Date",  value: "Date",  align: "start"},
          { text: "",  value: "",  align: "start"},
          { text: "",  value: "",  align: "start"},
          { text: "",  value: "",  align: "start"},
          { text: "",  value: "",  align: "start"}
        ];
        var csv = "";
        response.data.forEach((item) => {
          item.Values.forEach((item, index) => {
            if(item != null)
            {
              var isInHeader = false;
              headers.forEach((header) => {
                if(header.text == item.Name)
                {
                  isInHeader = true;
                }
              })
              if(!isInHeader)
              {
                headers[index + 1].text = item.Name;
                headers[index + 1].value = item.Name;
              }
            }
          });
        });
        headers.forEach((item, index) => {
          csv += item.text
          if(index != this.headers.length - 1)
            csv += ","
        });
        csv += '\n'
        response.data.forEach((item) => {
          var today = new Date(item.Date);
          var dd = String(today.getDate()).padStart(2, "0");
          var mm = String(today.getMonth() + 1).padStart(2, "0");
          var yyyy = today.getFullYear();
          var time =
            ("0" + today.getHours()).slice(-2) +
            ":" +
            ("0" + today.getMinutes()).slice(-2) +
            ":" +
            ("0" + today.getSeconds()).slice(-2);
          today = yyyy + "-" + mm + "-" + dd + " " + time;
          csv += today + ","
          item.Values.forEach((value, index) => {
            if(value == null)
            {
              csv += " ";
            }
            else
            {
              csv += value.Value;
            }
            if(index != item.Values.length - 1)
              csv += ","
          })
          csv += '\n'
        });
        console.log(csv);
        const anchor = document.createElement('a');
        anchor.href = 'data:text/csv;charset=utf-8,' + encodeURIComponent(csv);
        anchor.target = '_blank';
        anchor.download = this.dbSelect + '.csv';
        anchor.click();
      });
    }
  },
  created()
  {
    this.getCollections();
    
  },
  watch:{
    dbSelect()
    {
      this.isPaused = true;
      this.headers = [
        { text: "Date",  value: "Date",  align: "start"},
        { text: "",  value: "",  align: "start"},
        { text: "",  value: "",  align: "start"},
        { text: "",  value: "",  align: "start"},
        { text: "",  value: "",  align: "start"}
      ];
      this.id = [];
      this.data = [];
      this.chartData.labels = [];
      this.chartData.datasets = [
          {
            label: '',
            backgroundColor: '#80ccff',
            data: [],
            borderColor: "#80ccff"
          },
          {
            label: '',
            backgroundColor: '#ff8080',
            data: [],
            borderColor: "#ff8080"
          },
          {
            label: '',
            backgroundColor: '#80ff80',
            data: [],
            borderColor: "#80ff80"
          },
          {
            label: '',
            backgroundColor: '#ffff80',
            data: [],
            borderColor: "#ffff80"
          }
      ];
      
      setTimeout(() => {
        this.isPaused = false;
        // this.getData();
      }, 1250);
      setTimeout(() => {
        this.getData();
      }, 2000);
    }
  }
}
</script>

<style scoped>
#wrapper{
  height: 100%;
  widows: 100%;
  background-color: var(--v-primary-base);
  padding: 10px;
}
#tableBarText
{
  margin-top:7px;
  padding-left: 10px;
  padding-right: 10px;
}
.tableBar {
  padding: 10px;
  background-color: var(--v-accent-base);
  margin:auto;
  margin-bottom: 10px;
  width: 75%;
  
}
#dataTable{
  width: 75%;
  margin: auto;
}
#chartDiv{
  background-color: white;;
  margin:auto;
  width:75%;
  margin-bottom: 10px;
}
</style>