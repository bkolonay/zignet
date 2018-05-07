import React, { Component } from 'react';
import './testHistory.css';

class TestHistory extends Component {

  componentDidMount() {
    this.historyBarWidth = this.historyBarDiv.offsetWidth

    for (var i = 0; i < this.props.testFailureDurations.length; i++) {

    }
  }

  render() {
    return (
      <div
        className="testHistoryBar"
        ref={(div) => this.historyBarDiv = div}
      >
      <div style={{left: 25, width: 12}}/>
      
      </div>
    );
  }
}

export default TestHistory;

// {this.props.testFailureDurations.length}
//width: {this.historyBarWidth}