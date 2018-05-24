import React, { Component } from 'react';
import Chartist from 'chartist';
import './css/chartist.css';
import './css/chartistPieChart.css';

class ChartistPieChart extends Component {

  componentDidMount() {
  	this.chartistChart = Chartist.Pie(
  		this.chartContainer,
  		this.props.chartData,
  		{ 
  			donut: true,
			  donutWidth: 40
		  }
	  )
  }

  componentWillReceiveProps(props) {
  	this.chartistChart.update(props.chartData);
  }

  render() {
    return (
      <div 
        className="ct-chart" 
        id={"chart" + this.props.chartId}
        ref={(div) => this.chartContainer = div}
      />
    );
  }
}

export default ChartistPieChart;
