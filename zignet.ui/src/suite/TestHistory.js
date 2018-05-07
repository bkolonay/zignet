import React, { Component } from 'react';
import moment from 'moment';
import HistoryBarProvider from '../common/HistoryBarProvider'
import './testHistory.css';

class TestHistory extends Component {
  constructor() {
    super();
    this.state = {
      failureDivs: []
    };
    this.historyBarProvider = new HistoryBarProvider(); 
  }

  _getFailureDivs(testFailureDurations) {
    let historyBarWidth = this.historyBarDiv.offsetWidth
    let now = moment();
    var failureDivs = [];
    for (var i = 0; i < testFailureDurations.length; i++) {
      let failureDivAttributes = this.historyBarProvider.getErrorDivAttributes(historyBarWidth, now, testFailureDurations[i]);
      failureDivs.push(<div key={i} style={failureDivAttributes}/>);
    }
    return failureDivs;
  }

  componentDidMount() {
    this.setState({
      failureDivs: this._getFailureDivs(this.props.testFailureDurations)
    });
  }

  componentWillReceiveProps(props) {
    this.setState({
      failureDivs: this._getFailureDivs(props.testFailureDurations)
    });
  }

  render() {
    return (
      <div className="testHistoryBar"
           ref={(div) => this.historyBarDiv = div}>
      {this.state.failureDivs.map((failureDiv) => failureDiv)}      
      </div>
    );
  }
}

export default TestHistory;